using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.UserOperations.ManageUser;

public class UpdateUserRequestModel : IValidatableObject
{
    [JsonPropertyName("username")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Username { get; set; }
    
    [JsonPropertyName("email")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }

    [JsonPropertyName("first_name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string LastName { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Username) &&
            string.IsNullOrWhiteSpace(Email) &&
            string.IsNullOrWhiteSpace(FirstName) &&
            string.IsNullOrWhiteSpace(LastName))
        {
            yield return new ValidationResult("Pelo menos um dos campos deve ser preenchido.", new[] { nameof(Username), nameof(Email), nameof(FirstName), nameof(LastName) });
        }
    }
}
