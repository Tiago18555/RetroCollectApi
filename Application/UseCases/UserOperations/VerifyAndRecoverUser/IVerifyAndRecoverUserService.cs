namespace RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser
{
    public interface IVerifyAndRecoverUserService
    {
        void SendEmail(EmailDto request);
        string ChangePasswordTemplate(Guid userid);
    }
}

