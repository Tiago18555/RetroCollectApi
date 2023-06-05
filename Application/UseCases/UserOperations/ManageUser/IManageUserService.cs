using MimeKit;
using MailKit.Net.Smtp;
using RetroCollectApi.CrossCutting;
using System.Security.Claims;
using MimeKit.Text;
using MailKit.Security;

namespace RetroCollectApi.Application.UseCases.UserOperations.ManageUser
{
    public interface IManageUserService
    {
        ResponseModel UpdateUser(UpdateUserRequestModel user, ClaimsPrincipal claim);
        ResponseModel UpdatePassword(UpdatePwdRequestModel user);
        Task<ResponseModel> GetAllUsers();
    }
    public interface IVerifyAndRecoverUserService
    {
        
    }
    public class VerifyAndRecoverUserService : IVerifyAndRecoverUserService
    {
        private readonly IConfiguration _config;

        public VerifyAndRecoverUserService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }

    public struct EmailDto
    {
        public EmailDto()
        {

        }
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}

