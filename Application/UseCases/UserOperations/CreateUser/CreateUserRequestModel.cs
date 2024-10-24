using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CrossCutting.Validations;

namespace Application.UseCases.UserOperations.CreateUser;

public class CreateUserRequestModel
{
    [JsonPropertyName("username")]
    [IsValidUri]
    [MinLength(3)]
    [MaxLength(255)]
    [Required(ErrorMessage = "O campo \"username\" é obrigatório")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    [Required(ErrorMessage = "\"O campo \"password\" é obrigatório\"")]
    [MinLength(6)]
    [MaxLength(255)]
    public string Password { get; set; }

    [JsonPropertyName("confirm_password")]
    [Required(ErrorMessage = "\"O campo \"password\" é obrigatório\"")]
    [MinLength(6)]
    [MaxLength(255)]
    [Compare("Password", ErrorMessage = "Os campos \"password\" e \"confirm password\" devem ser iguais")]
    public string ConfirmPassword { get; set; }

    [JsonPropertyName("email")]
    [Required(ErrorMessage = "\"O campo \"email\" é obrigatório\"")]
    [EmailAddress(ErrorMessage = "O campo \"email\" está com o formato inválido")]
    public string Email { get; set; }

    [JsonPropertyName("first_name")]
    [MinLength(3)]
    [MaxLength(255)]
    [Required(ErrorMessage = "\"O campo \"first name\" é obrigatório\"")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string LastName { get; set; }
}
