using Domain.Entities;

namespace Application.Processors.UserOperations.VerifyAndRecoverUser;

public class SendEmailInfo
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string TimeStampHash { get; set; }
}

