using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.UserOperations.VerifyAndRecoverUser;
public class UpdatePasswordRequestModel
{
    [JsonPropertyName("password")]
    [Required]
    public string Password { get; set; }

    [JsonPropertyName("confirm_password")]
    [Required, Compare("Password")]
    public string ConfirmPassword { get; set; }
}

