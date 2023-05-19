using System.ComponentModel.DataAnnotations;

namespace RetroCollectApi.Application.UseCases.UserOperations.Authenticate
{
    public class AuthenticateServiceRequestModel
    {
        [MinLength(3)]
        [MaxLength(20)]
        [Required(ErrorMessage = "O campo usuário não pode estar vazio")]
        public string Username { get; set; }

        [MinLength(6)]
        [Required(ErrorMessage = "O campo senha não pode estar vazio.")]
        public string Password { get; set; }

    }
}
