using System.ComponentModel.DataAnnotations;

namespace Application.Processor.UserOperations.CreateUser
{
    public class CreateUserRequestModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
