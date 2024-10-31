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

    public async Task<ResponseModel> VerifyUser(string username)
    {
        var user = _userRepository.SingleOrDefault(r => r.Username == username);

        if (String.IsNullOrEmpty(username))
            return ResponseFactory.BadRequest($"User not found");

        if (user == null)
            return ResponseFactory.NotFound($"User {username} not found");

        if (user.VerifiedAt != DateTime.MinValue)
            return "This user is already verified".Ok();

        var messageObject = new MessageModel 
        { 
            Message = user,
            SourceType = "verify-user" 
        };

        var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "recover");

        var data = JsonSerializer.Deserialize (
            message, 
            typeof(MessageModel)
        ) as MessageModel;
        
        return "Verified".Ok(message = status);     
    }

    public async Task<ResponseModel> SendEmail(SendEmailRequestModel request, CancellationToken cts)
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

        (bool canAttempt, int timeToWait) = await CanAttemptPasswordRecovery(foundUser.Username, cts);

        if (canAttempt == false)          
            return ResponseFactory.BadRequest($"This operation cannot be run now. Wait {timeToWait} seconds");

        try
        {
            var messageObject = new MessageModel 
            { 
                Message = new SendEmailInfo 
                { 
                    TimeStampHash = timestampHash, 
                    Email = foundUser.Email, 
                    Username = foundUser.Username 
                }, 
                SourceType = "recover-user" 
            };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "recover");

            var data = JsonSerializer.Deserialize (
                message, 
                typeof(MessageModel)
            ) as MessageModel;
            
            return "Success".Ok(message = status);
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
        string id = Guid.NewGuid().ToString();

        string combinedString = $"{timestamp}-{id}";

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

    private async Task<(bool CanAttempt, int WaitTimeRequired)> CanAttemptPasswordRecovery(string username, CancellationToken cts)
    {
        int waitTimeRequired = 0;
        double baseTimeAcumulator = double.Parse(_config.GetSection("RecoverConfigs:PasswordChangeTries:InitialBackoffInSeconds").Value);
        int maxTimeAcumulated = int.Parse(_config.GetSection("RecoverConfigs:PasswordChangeTries:MaxBackoffInSeconds").Value);

        var lastAttempt = await _recoverRepository.FindDocument("RecoverCollection", "Username", username, cts);

        if (lastAttempt != null)
        {
            var timestampString = lastAttempt["TimeStampHash"].ToString().Split('-')[0];

            if (long.TryParse(timestampString, out var timestamp))
            {
                var attemptTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
                var timeSinceLastAttempt = _timeProvider.UtcNow - attemptTime;

                int secondsSinceLastAttempt = (int)timeSinceLastAttempt.TotalSeconds;
                int failedAttempts = GetFailedAttemptsCount(username);

                int nextWait = (int)Math.Min(Math.Pow(baseTimeAcumulator, failedAttempts) * 60, maxTimeAcumulated);

                waitTimeRequired = nextWait;

                if (secondsSinceLastAttempt <= waitTimeRequired)
                {
                    return (false, waitTimeRequired - secondsSinceLastAttempt);
                }
            }
        }
        return (true, 0);
    }

    private int GetFailedAttemptsCount(string username) =>   
        _recoverRepository.CountFailedAttemptsSinceLastSuccess(username);
    

    #endregion

    public ResponseModel ChangePasswordTemplate(string username, string timestampHash)
    {
        var foundUser = _userRepository.SingleOrDefault(u => u.Username == username);
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
        var resetLink = $"{host}auth/update/{foundUser.Username}/{timestampHash}";

        var res = template
            .Replace("#userName", foundUser.Username)
            .Replace("#url*", resetLink);

        StdOut.Info(res);

        return res.Ok();
    }

    public async Task<ResponseModel> ChangePassword(string username, UpdatePasswordRequestModel pwd, string timestampHash)
    {
        var foundUser = _userRepository.SingleOrDefault(u => u.Username == username);

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
                    TimestampHash = timestampHash, 
                    Username = foundUser.Username, 
                    Password = pwd.Password
                }, 
                SourceType = "change-password" 
            };

            var (status, message) = await _producer.SendMessage(JsonSerializer.Serialize(messageObject), "recover");

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

