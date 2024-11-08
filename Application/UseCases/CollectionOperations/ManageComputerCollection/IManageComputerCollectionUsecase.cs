using System.Security.Claims;
using Domain;
using Application.UseCases.CollectionOperations.Shared;

namespace Application.UseCases.CollectionOperations.ManageComputerCollection;

public interface IManageComputerCollectionUsecase
{
    public Task<ResponseModel> AddComputer(AddItemRequestModel item, ClaimsPrincipal user);
    public Task<ResponseModel> DeleteComputer(Guid id, ClaimsPrincipal user);
    public Task<ResponseModel> UpdateComputer(UpdateComputerRequestModel item, ClaimsPrincipal user);
    public Task<ResponseModel> GetAllComputersByUser(ClaimsPrincipal user);

}
