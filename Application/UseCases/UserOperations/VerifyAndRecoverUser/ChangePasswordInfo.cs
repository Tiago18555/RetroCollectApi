namespace Application.UseCases.UserOperations.VerifyAndRecoverUser;

public class ChangePasswordInfo
{
    public string username { get; set; }
    public string password { get; set; }
    public string timestampHash { get; set; }
}

