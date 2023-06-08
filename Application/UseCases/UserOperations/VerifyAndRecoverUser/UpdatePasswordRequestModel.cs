using System.ComponentModel.DataAnnotations;

namespace RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser
{
    public partial class VerifyAndRecoverUserService
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

