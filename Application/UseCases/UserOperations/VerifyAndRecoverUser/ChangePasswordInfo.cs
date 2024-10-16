namespace Application.UseCases.UserOperations.VerifyAndRecoverUser;

public class ChangePasswordInfo
{
    public Guid userId { get; set; }
    public string password { get; set; }
    public string timestampHash { get; set; }
}

