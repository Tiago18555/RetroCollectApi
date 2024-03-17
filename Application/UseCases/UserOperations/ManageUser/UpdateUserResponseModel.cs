namespace Application.UseCases.UserOperations.ManageUser
{
    public class UpdateUserResponseModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
