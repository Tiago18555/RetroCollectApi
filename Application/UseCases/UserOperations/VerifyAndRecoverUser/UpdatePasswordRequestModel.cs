using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.UserOperations.VerifyAndRecoverUser
{
    public partial class VerifyAndRecoverUserUsecase
    {
        public class UpdatePasswordRequestModel
        {
            [Required]
            public string Password { get; set; }

            [Required, Compare("Password")]
            public string ConfirmPassword { get; set; }
        }
    }
}

