using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.UserOperations.ManageUser
{
    public interface IManageUserService
    {
        ResponseModel UpdateUser(UpdateUserRequestModel user);
        ResponseModel UpdatePassword(UpdatePwdRequestModel user);
    }
}
