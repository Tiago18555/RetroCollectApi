namespace Application.UseCases.UserOperations.VerifyAndRecoverUser
{
    public partial class VerifyAndRecoverUserService
    {
        public class PasswordResetInfo
        {
            public Guid UserId { get; set; }
            public string Hash { get; set; }
            public DateTime Timestamp { get; set; }
            public bool Success { get; set; }
        }
    }
}

