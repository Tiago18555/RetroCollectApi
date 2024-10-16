using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Configuration;
using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using CrossCutting.Providers;
using CrossCutting;
using Domain.Broker;
using System.Text.Json;
using System.Data;
using Application.Processors.UserOperations.VerifyAndRecoverUser;

namespace Application.UseCases.UserOperations.VerifyAndRecoverUser;

public partial class VerifyAndRecoverUserUsecase : IVerifyAndRecoverUserUsecase
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;
    private readonly IRecoverRepository _recoverRepository;
    private readonly IDateTimeProvider _timeProvider;
    private readonly IProducerService _producer;

    public VerifyAndRecoverUserUsecase (
        IConfiguration config, 
        IUserRepository repository, 
        IRecoverRepository recoverRepository, 
        IDateTimeProvider timeProvider,
        IProducerService producer
    )
    {
        _config = config;
        _userRepository = repository;
        _recoverRepository = recoverRepository;
        _timeProvider = timeProvider;
        _producer = producer;
    }

    public ResponseModel VerifyUser(Guid userId)
    {
        var user = _userRepository.SingleOrDefault(r => r.UserId == userId);
        if (user.VerifiedAt != DateTime.MinValue)
            return "This user is already verified".Ok();
        user.VerifiedAt = _timeProvider.UtcNow;
        return _userRepository
            .Update(user)
            .MapObjectTo( new VerifyUserResponseModel() )
            .Ok();        
    }

    public async Task<ResponseModel> SendEmail(SendEmailRequestModel request)
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
            _userRepository.SingleOrDefault(u => u.Email == request.Email) : 
            _userRepository.SingleOrDefault(u => u.Username == request.UserName);

        if (foundUser == null)
            return ResponseFactory.NotFound("User not found");
        
        string timestampHash = GenerateTimestampHash();

        if (!IsValidTimestampHash(timestampHash))            
            return ResponseFactory.NotFound("Hash expired or invalid");        

        (bool canAttempt, int timeToWait) = CanAttemptPasswordRecovery(foundUser.UserId);

        if (canAttempt == false)          
            return ResponseFactory.BadRequest($"This operation cannot be run now. Wait {timeToWait} seconds");

        try
        {
            var messageObject = new MessageModel 
            { 
                Message = new SendEmailInfo 
                { 
                    TimeStampHash = timestampHash, 
                    UserId = foundUser.UserId, 
                    Email = foundUser.Email, 
                    Username = foundUser.Username 
                }, 
                SourceType = "verify-recover-user" 
            };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject));

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Success".Created(message = status);
        }
        catch (DBConcurrencyException)
        {
            return ResponseFactory.NotAcceptable("Formato de dados inválido");
        }
        catch (InvalidOperationException)
        {
            return ResponseFactory.NotAcceptable("Formato de dados inválido.");
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region inner methods

    private string GenerateTimestampHash()
    {
        long timestamp = _timeProvider.GetCurrentTimestampSeconds();
        string uniqueIdentifier = Guid.NewGuid().ToString();

        string combinedString = $"{timestamp}-{uniqueIdentifier}";

        return combinedString;
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
        var foundUser = _userRepository.SingleOrDefault(u => u.UserId == userId);
        if (foundUser == null) return ResponseFactory.NotFound("User not found");

        string host = _config.GetSection("Host").Value;

        var template = File.ReadAllText(

            Path.Combine(
                Environment.CurrentDirectory,
                _config["BasePath"],
                "Static",
                "change-password.html"
            )

        );
        var resetLink = $"{host}auth/update/{foundUser.UserId}/{timestampHash}";

        var res = template
            .Replace("#userName", foundUser.Username)
            .Replace("#url*", resetLink);

        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine(res);
        System.Console.ForegroundColor = ConsoleColor.White;

        return res.Ok();
    }

    public async Task<ResponseModel> ChangePassword(Guid userId, UpdatePasswordRequestModel pwd, string timestampHash)
    {
        var foundUser = _userRepository.SingleOrDefault(u => u.UserId == userId);

        if (foundUser == null)            
            return ResponseFactory.NotFound("User Not Found");            

        if (!IsValidTimestampHash(timestampHash))            
            return "Password reset request has expired or is invalid".Ok();

        if (BCryptNet.Verify(pwd.Password, foundUser.Password))            
            return "New password cannot be equal to the old one".Ok();

        try
        {
            var messageObject = new MessageModel
            { 
                Message = new ChangePasswordInfo 
                { 
                    timestampHash = timestampHash, 
                    userId = foundUser.UserId, 
                    password = pwd.Password
                }, 
                SourceType = "change-password" 
            };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject));

            var data = JsonSerializer.Deserialize(
                message, 
                typeof( MessageModel )
            ) as MessageModel;

            return "Success".Ok();
        }
        catch (Exception)
        {
            throw;
        }
    }
}

