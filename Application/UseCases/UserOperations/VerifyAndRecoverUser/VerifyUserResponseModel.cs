namespace RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser
{
    public class VerifyUserResponseModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime VerifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

