/*using System.Data;
using BCryptNet = BCrypt.Net.BCrypt;
using Application.Processor.UserOperations.CreateUser;
using CrossCutting;
using Domain.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

public class CreateUserProcessor
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _config;
    public CreateUserProcessor(IUserRepository repository, IConfiguration config, IKafkaProducerService producer, IKafkaConsumerService consumer)
    {
        this._repository = repository;
        this._config = config;
    }

    public object BCryptNet { get; private set; }

    public void CreateUser(CreateUserRequestModel createUserRequestModel)
    {
        User user = createUserRequestModel.MapObjectTo(new User());

        if (_repository.Any(x => x.Username == createUserRequestModel.Username || x.Email == createUserRequestModel.Email))
        {
            return ResponseFactory.Conflict();
        }

        user.Password = BCryptNet.HashPassword(createUserRequestModel.Password);
        user.CreatedAt = DateTime.Now;

        try
        {
            var newUser = this._repository.Add(user)

            // Consumer
            SendEmailToVerify(newUser.Data as CreateUserResponseModel);
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

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public string SendEmailToVerify(CreateUserResponseModel user)
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
        email.Subject = "RetroCollect Password Recover";
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        smtp.Connect(_config.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(_config.GetSection("Email:Username").Value, _config.GetSection("Email:Password").Value);
        smtp.Send(email);
        smtp.Disconnect(true);

        return "Email sent";
        
    }
}

*/