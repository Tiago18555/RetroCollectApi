using System.Data;
using BCryptNet = BCrypt.Net.BCrypt;
using CrossCutting;
using Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace Application.UseCases.UserOperations.CreateUser
{
    public class CreateUserService : ICreateUserService
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _config;
        public CreateUserService(IUserRepository repository, IConfiguration config)
        {
            this._repository = repository;
            this._config = config;
        }

        public ResponseModel CreateUser(CreateUserRequestModel createUserRequestModel)
        {
            User user = createUserRequestModel.MapObjectTo(new User());

            if (_repository.Any(x => x.Username == createUserRequestModel.Username || x.Email == createUserRequestModel.Email))
            {
                return GenericResponses.Conflict();
            }

            user.Password = BCryptNet.HashPassword(createUserRequestModel.Password);
            user.CreatedAt = DateTime.Now;

            try
            {
                var newUser = this._repository.Add(user)
                    .MapObjectTo(new CreateUserResponseModel())
                    .Created();

                SendEmailToVerify(newUser.Data as CreateUserResponseModel);

                return newUser;
            }
            catch (DBConcurrencyException)
            {
                return GenericResponses.NotAcceptable("Formato de dados inválido");
            }
            catch (InvalidOperationException)
            {
                return GenericResponses.NotAcceptable("Formato de dados inválido.");
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
                    System.Environment.CurrentDirectory,
                    "..",
                    "Application",
                    "UseCases",
                    "UserOperations",
                    "CreateUser",
                    "Resources",
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
}
