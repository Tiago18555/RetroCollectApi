using Application.CrossCutting;
using System.Security.Claims;

namespace Application.UseCases.UserOperations.ManageUser
{
    public interface IManageUserService
    {
        ResponseModel UpdateUser(UpdateUserRequestModel user, ClaimsPrincipal claim);
        Task<ResponseModel> GetAllUsers();
    }
}

