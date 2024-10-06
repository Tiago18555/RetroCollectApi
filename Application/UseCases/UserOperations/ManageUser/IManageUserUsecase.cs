using CrossCutting;
using System.Security.Claims;

namespace Application.UseCases.UserOperations.ManageUser
{
    public interface IManageUserUsecase
    {
        ResponseModel UpdateUser(UpdateUserRequestModel user, ClaimsPrincipal claim);
        Task<ResponseModel> GetAllUsers();
    }
}

