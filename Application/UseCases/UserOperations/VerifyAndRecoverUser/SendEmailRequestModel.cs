using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.UserOperations.VerifyAndRecoverUser;

public class SendEmailRequestModel
{
    [EmailAddress]
    public string Email { get; set; }
    public string UserName { get; set; }
}

