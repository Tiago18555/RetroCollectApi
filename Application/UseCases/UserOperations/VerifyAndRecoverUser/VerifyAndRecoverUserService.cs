using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using MailKit.Security;
using RetroCollectApi.Repositories.Interfaces;
using RetroCollect.Models;
using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser
{
    public class VerifyAndRecoverUserService : IVerifyAndRecoverUserService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _repository;

        public VerifyAndRecoverUserService(IConfiguration config, IUserRepository repository)
        {
            _config = config;
            _repository = repository;
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


            //string emailSentId = new Guid().ToString();

            string host = _config.GetSection("Host").Value;

            var template = File.ReadAllText($@"{System.Environment.CurrentDirectory}\Application\UseCases\UserOperations\VerifyAndRecoverUser\recover-template.html");
            var body = template.Replace("#resetLink", $"{host}api/auth/recover/{foundUser.UserId}");

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

        public ResponseModel ChangePasswordTemplate(Guid userid)
        {
            var username = _repository.SingleOrDefault(u => u.UserId == userid);
            if (username == null) return GenericResponses.NotFound("User not found");

            var template = File.ReadAllText($@"{Environment.CurrentDirectory}\Application\UseCases\UserOperations\VerifyAndRecoverUser\change-password.html");
            var res = template.Replace("#userName", username.Username);

            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(res);
            System.Console.ForegroundColor = ConsoleColor.White;

            return res.Ok();
        }
    }
}

