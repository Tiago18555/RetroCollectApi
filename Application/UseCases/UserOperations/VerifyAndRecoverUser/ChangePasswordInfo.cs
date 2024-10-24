namespace Application.UseCases.UserOperations.VerifyAndRecoverUser;

public class ChangePasswordInfo
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string TimestampHash { get; set; }
}

