using RetroCollectApi.CrossCutting;
using static RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser.VerifyAndRecoverUserService;

namespace RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser
{
    public interface IVerifyAndRecoverUserService
    {
        ResponseModel SendEmail(SendEmailRequestModel request);
        ResponseModel ChangePasswordTemplate(Guid userid);
        ResponseModel ChangePassword(Guid user_id, UpdatePasswordRequestModel pwd);
    }
}

