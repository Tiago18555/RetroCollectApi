using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.UserOperations.ManageUser;

public class UpdateUserRequestModel : IValidatableObject
{
    [MinLength(3)]
    [MaxLength(255)]
    public string Username { get; set; }

    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }

    [MinLength(3)]
    [MaxLength(255)]
    public string FirstName { get; set; }

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
