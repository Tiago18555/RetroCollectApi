using Domain.Broker;
using Domain.Entities;

using MimeKit;
using System.Data;
using MimeKit.Text;
using System.Text.Json;
using MailKit.Security;
using MailKit.Net.Smtp;
using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Configuration;
using Application.UseCases.UserOperations.CreateUser;
using Domain.Repositories;
using CrossCutting;

namespace Application.Processor.UserOperations.CreateUser;
public class CreateUserProcessor : IRequestProcessor
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _config;
    public CreateUserProcessor (
        IUserRepository repository, 
        IConfiguration config
    )
    {
        this._repository = repository;
        this._config = config;
    }

    public async Task<MessageModel> ProcessAsync(string message)
    {
        System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
        System.Console.WriteLine("STARTING PROCESS...");
        System.Console.WriteLine(message);
        System.Console.ForegroundColor = ConsoleColor.White;

        var field = message.ExtractMessage();

        System.Console.WriteLine(field);

        var request = JsonSerializer.Deserialize<CreateUserRequest>(field);

        var res = await CreateUser(request);

        return new MessageModel{ Message = res, SourceType = "create-user-response" };
    }

    /// <exception cref="DBConcurrencyException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="System.Reflection.TargetException"></exception>
    /// <exception cref="System.Reflection.TargetInvocationException"></exception>
    /// <exception cref="MethodAccessException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    /// <exception cref="BCrypt.Net.SaltParseException"></exception>
    /// <exception cref="DbUpdateConcurrencyException"></exception>
    /// <exception cref="DBUpdateException"></exception>
    public async Task<CreateUserResponseModel> CreateUser(CreateUserRequest createUserRequestModel)
    {
        User user = createUserRequestModel.MapObjectTo(new User());

        user.Password = BCryptNet.HashPassword(createUserRequestModel.Password);
        user.CreatedAt = DateTime.Now;

        var newUser = this._repository.Add(user);

        SendEmailToVerify(newUser);

        return newUser.MapObjectTo(new CreateUserResponseModel());
    }


    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    internal void SendEmailToVerify(User user)
    {
        if (string.IsNullOrEmpty(user.Email))
        {
            throw new ArgumentException($"Valor de email não pode ser nulo. at {System.Environment.CurrentDirectory}");
        }
        string host = _config.GetSection("Host").Value;

        var verificationLink = $"{host}auth/verify/{user.UserId}";

        var template = File.ReadAllText(
            Path.Combine(
                _config["BasePath"],
                "Static",
                "verify-template.html"
            )
        );

        var body = template
            .Replace("#verificationLink", verificationLink)
            .Replace("#userName", user.Username);

        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine(body);
        System.Console.ForegroundColor = ConsoleColor.White;

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:Username").Value));
        email.To.Add(MailboxAddress.Parse(user.Email));
        email.Subject = "RetroCollect user verification";
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        smtp.Connect(_config.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(_config.GetSection("Email:Username").Value, _config.GetSection("Email:Password").Value);
        smtp.Send(email);
        smtp.Disconnect(true);

        System.Console.ForegroundColor = ConsoleColor.Blue;
        System.Console.WriteLine("Email sent");        
        System.Console.ForegroundColor = ConsoleColor.White;
    }

}
