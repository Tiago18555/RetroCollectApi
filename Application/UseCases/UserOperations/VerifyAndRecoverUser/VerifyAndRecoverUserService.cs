using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using MailKit.Security;
using System;
using RetroCollectApi.Repositories.Interfaces;

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

        public void SendEmail(EmailDto request)
        {
            //string emailSentId = new Guid().ToString();
            string host = _config.GetSection("Host").Value;

            var template = File.ReadAllText($"{Environment.CurrentDirectory}\\Application\\UseCases\\UserOperations\\VerifyAndRecoverUser\\recover-template.html");
            var body = template.Replace("#resetLink", $"{host}api/auth/recover/{request.UserId}");

            Console.WriteLine(body);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:Username").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = "RetroCollect Password Recover";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("Email:Host").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("Email:Username").Value, _config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public string ChangePasswordTemplate(Guid userid)
        {
            var username = _repository.SingleOrDefault(u => u.UserId == userid);
            if (username == null) throw new Exception("User not found on VerifyAndRecoverUser: 44");
            var template = File.ReadAllText($"{Environment.CurrentDirectory}\\Application\\UseCases\\UserOperations\\VerifyAndRecoverUser\\change-password.html");
            var res = template.Replace("#userName", username.Username);

            Console.WriteLine(res);

            return res;
        }
    }
}

