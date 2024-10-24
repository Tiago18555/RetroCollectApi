using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.UserOperations.VerifyAndRecoverUser;

public class SendEmailRequestModel
{
    [JsonPropertyName("email")]
    [EmailAddress]
    public string Email { get; set; }

    [JsonPropertyName("username")]
    public string UserName { get; set; }
}

