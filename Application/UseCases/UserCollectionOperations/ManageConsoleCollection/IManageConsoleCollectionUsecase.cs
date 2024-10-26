using Domain;
using Application.UseCases.UserCollectionOperations.Shared;
using System.Security.Claims;

namespace Application.UseCases.UserCollectionOperations.ManageConsoleCollection;

public interface IManageConsoleCollectionUsecase
{
    public Task<ResponseModel> AddConsole(AddItemRequestModel item, ClaimsPrincipal user);
    public Task<ResponseModel> DeleteConsole(Guid id, ClaimsPrincipal user);
    public Task<ResponseModel> UpdateConsole(UpdateConsoleRequestModel item, ClaimsPrincipal user);
    public Task<ResponseModel> GetAllConsolesByUser(ClaimsPrincipal user);

}
