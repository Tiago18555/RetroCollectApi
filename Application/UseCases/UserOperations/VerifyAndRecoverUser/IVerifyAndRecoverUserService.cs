using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser
{
    public interface IVerifyAndRecoverUserService
    {
        ResponseModel SendEmail(SendEmailRequestModel request);
        ResponseModel ChangePasswordTemplate(Guid userid);
    }
}

