using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using MailKit.Security;
using RetroCollectApi.Repositories.Interfaces;
using RetroCollect.Models;
using RetroCollectApi.CrossCutting;
using BCryptNet = BCrypt.Net.BCrypt;
using RetroCollectApi.Application.UseCases.UserOperations.Authenticate;
using Microsoft.Extensions.Hosting;
using System.Text;
using MongoDB.Bson;
using RetroCollectApi.CrossCutting.Providers;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;

namespace RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser
{
    public partial class VerifyAndRecoverUserService : IVerifyAndRecoverUserService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _repository;
        private readonly IRecoverRepository _recoverRepository;
        private readonly IDateTimeProvider _timeProvider;

        public VerifyAndRecoverUserService(IConfiguration config, IUserRepository repository, IRecoverRepository recoverRepository, IDateTimeProvider timeProvider)
        {
            _config = config;
            _repository = repository;
            _recoverRepository = recoverRepository;
            _timeProvider = timeProvider;
        }

        public ResponseModel VerifyUser(Guid userId)
        {
            var user = _repository.SingleOrDefault(r => r.UserId == userId);
            if (user.VerifiedAt != DateTime.MinValue)
                return "This user is already verified".Ok();
            user.VerifiedAt = _timeProvider.UtcNow;
            return _repository
                .Update(user)
                .MapObjectTo( new VerifyUserResponseModel() )
                .Ok();        
        }

        public ResponseModel SendEmail(SendEmailRequestModel request)
        {
            if (string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.UserName))
            {
                throw new ArgumentException("Deve ser fornecido um Email ou UserName.");
            }

            if (!string.IsNullOrEmpty(request.Email) && !string.IsNullOrEmpty(request.UserName))
            {
                throw new ArgumentException("Deve ser fornecido apenas um Email ou UserName.");
            }

            User foundUser = 
                !string.IsNullOrEmpty(request.Email) ? 
                _repository.SingleOrDefault(u => u.Email == request.Email) : 
                _repository.SingleOrDefault(u => u.Username == request.UserName);

            if (foundUser == null) return GenericResponses.NotFound("User not found");

            string host = _config.GetSection("Host").Value;

            string timestampHash = GenerateTimestampHash();
            var resetInfo = new PasswordResetInfo
            {
                UserId = foundUser.UserId,
                Hash = timestampHash,
                Timestamp = _timeProvider.UtcNow,
                Success = false
            };

            if (!IsValidTimestampHash(resetInfo.Hash))            
                return GenericResponses.NotFound("Hash expired or invalid");

            (bool canAttempt, int timeToWait) = CanAttemptPasswordRecovery(resetInfo.UserId);

            if (canAttempt == false)          
                return GenericResponses.BadRequest($"This operation cannot be run now. Wait {timeToWait} seconds");            
                    

            _recoverRepository.InsertDocument("RecoverCollection", resetInfo.ToBsonDocument());

            var resetLink = $"{host}api/auth/recover/{foundUser.UserId}/{timestampHash}";


            var template = File.ReadAllText(

                Path.Combine(
                    System.Environment.CurrentDirectory,
                    "Application", 
                    "UseCases", 
                    "UserOperations", 
                    "VerifyAndRecoverUser", 
                    "Resources", 
                    "recover-template.html"
                )

            );

            var body = template
                .Replace("#resetLink", resetLink)
                .Replace("#userName", foundUser.Username);

            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(body);
            System.Console.ForegroundColor = ConsoleColor.White;

            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:Username").Value));
                email.To.Add(MailboxAddress.Parse(foundUser.Email));
                email.Subject = "RetroCollect Password Recover";
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("Email:Username").Value, _config.GetSection("Email:Password").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                return "Email sent".Ok();
            }
            catch (Exception msg) { return GenericResponses.ServiceUnavailable(msg.ToString()); }
        }

        #region inner methods

        private string GenerateTimestampHash()
        {
            long timestamp = _timeProvider.GetCurrentTimestampSeconds();
            string uniqueIdentifier = Guid.NewGuid().ToString();

            string combinedString = $"{timestamp}-{uniqueIdentifier}";

            return combinedString;
        }

        public bool PasswordRecoveryHashExists(string userId, string hash)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("UserId", userId) & Builders<BsonDocument>.Filter.Eq("Hash", hash);
            return _recoverRepository.Any("RecoverCollection", filter);
        }


        /// <summary>
        /// Verifies if a time stamp hash is expired
        /// </summary>
        /// <param name="timestampHash"></param>
        /// <returns>true if time stamp hash is not expired</returns>
        private bool IsValidTimestampHash(string timestampHash)
        {
            int default_expires_at_value = 10;

            string expires_at = _config.GetSection("RecoverConfigs:PasswordResetTokenValidityMinutes").Value;

            if (string.IsNullOrEmpty(expires_at) || !int.TryParse(expires_at, out default_expires_at_value))
                throw new ConfigurationException("PasswordResetTokenValidityMinutes configuration is invalid");
            

            var timestampParts = timestampHash.Split('-');

            if (long.TryParse(timestampParts[0], out var requestTimestamp))
            {
                var currentTimestamp = _timeProvider.GetCurrentTimestampSeconds();
                long expirationTime = requestTimestamp + (int.Parse(expires_at) * 60);

                return currentTimestamp <= expirationTime;
            }

            return false;
        }

        public (bool CanAttempt, int WaitTimeRequired) CanAttemptPasswordRecovery(Guid userId)
        {
            int waitTimeRequired = 0;
            double baseTimeAcumulator = double.Parse(_config.GetSection("RecoverConfigs:PasswordChangeTries:InitialBackoffInSeconds").Value);
            int maxTimeAcumulated = int.Parse(_config.GetSection("RecoverConfigs:PasswordChangeTries:MaxBackoffInSeconds").Value);

            var lastAttempt = _recoverRepository.FindDocument("RecoverCollection", "UserId", userId);

            if (lastAttempt != null)
            {
                var attemptTime = lastAttempt["Timestamp"].ToUniversalTime();
                var timeSinceLastAttempt = _timeProvider.UtcNow - attemptTime;

                int secondsSinceLastAttempt = (int)timeSinceLastAttempt.TotalSeconds;
                int failedAttempts = GetFailedAttemptsCount(userId);

                var nextWait = (int)Math.Pow(baseTimeAcumulator / 60, failedAttempts)*60;

                waitTimeRequired =
                    nextWait > maxTimeAcumulated
                    ? maxTimeAcumulated
                    : nextWait;

                if (secondsSinceLastAttempt <= waitTimeRequired)
                    return (false, waitTimeRequired - secondsSinceLastAttempt);
            }
            return (true, 0);
        }

        private int GetFailedAttemptsCount(Guid userId) =>   
            _recoverRepository.CountFailedAttemptsSinceLastSuccess(userId);
        

        #endregion

        public ResponseModel ChangePasswordTemplate(Guid userId, string timestampHash)
        {
            var foundUser = _repository.SingleOrDefault(u => u.UserId == userId);
            if (foundUser == null) return GenericResponses.NotFound("User not found");

            string host = _config.GetSection("Host").Value;

            var template = File.ReadAllText(

                Path.Combine(
                    Environment.CurrentDirectory, 
                    "Application", 
                    "UseCases", 
                    "UserOperations", 
                    "VerifyAndRecoverUser", 
                    "Resources", 
                    "change-password.html"
                )

            );
            var resetLink = $"{host}api/auth/update/{foundUser.UserId}/{timestampHash}";

            var res = template
                .Replace("#userName", foundUser.Username)
                .Replace("#url*", resetLink);

            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(res);
            System.Console.ForegroundColor = ConsoleColor.White;

            return res.Ok();
        }

        public ResponseModel ChangePassword(Guid userId, UpdatePasswordRequestModel pwd, string timestampHash)
        {
            var foundUser = _repository.SingleOrDefault(u => u.UserId == userId);

            if (foundUser == null)            
                return GenericResponses.NotFound("User Not Found");            

            if (!IsValidTimestampHash(timestampHash))            
                return "Password reset request has expired or is invalid".Ok();
            
            var hashedNewPassword = BCryptNet.HashPassword(pwd.Password);

            if (BCryptNet.Verify(pwd.Password, foundUser.Password))            
                return "New password cannot be equal to the old one".Ok();           

            foundUser.Password = hashedNewPassword;
            foundUser.UpdatedAt = _timeProvider.UtcNow;

            try
            {
                _repository.Update(foundUser);
                _recoverRepository.UpdateDocument("RecoverCollection", "UserId", foundUser.UserId, "Success", true);
                return "Password updated successfully".Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

