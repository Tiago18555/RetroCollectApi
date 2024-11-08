using Domain;
using Application.UseCases.CollectionOperations.Shared;
using System.Security.Claims;

namespace Application.UseCases.CollectionOperations.ManageConsoleCollection;

public interface IManageConsoleCollectionUsecase
{
    public Task<ResponseModel> AddConsole(AddItemRequestModel item, ClaimsPrincipal user);
    public Task<ResponseModel> DeleteConsole(Guid id, ClaimsPrincipal user);
    public Task<ResponseModel> UpdateConsole(UpdateConsoleRequestModel item, ClaimsPrincipal user);
    public Task<ResponseModel> GetAllConsolesByUser(ClaimsPrincipal user);

}
