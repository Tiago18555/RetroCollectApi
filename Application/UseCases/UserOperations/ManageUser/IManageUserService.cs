using RetroCollectApi.CrossCutting;
using System.Security.Claims;

namespace RetroCollectApi.Application.UseCases.UserOperations.ManageUser
{
    public interface IManageUserService
    {
        ResponseModel UpdateUser(UpdateUserRequestModel user, ClaimsPrincipal claim);
        ResponseModel UpdatePassword(UpdatePwdRequestModel user);
        Task<ResponseModel> GetAllUsers();
    }
}

