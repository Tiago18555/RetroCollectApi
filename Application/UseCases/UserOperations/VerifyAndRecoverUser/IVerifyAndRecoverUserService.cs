using RetroCollectApi.CrossCutting;
using static RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser.VerifyAndRecoverUserService;

namespace RetroCollectApi.Application.UseCases.UserOperations.VerifyAndRecoverUser
{
    public interface IVerifyAndRecoverUserService
    {
        ResponseModel SendEmail(SendEmailRequestModel request);
        ResponseModel VerifyUser(Guid userid);
        ResponseModel ChangePasswordTemplate(Guid userid, string timestamphash);
        ResponseModel ChangePassword(Guid user_id, UpdatePasswordRequestModel pwd, string timestamphash);
    }
}

