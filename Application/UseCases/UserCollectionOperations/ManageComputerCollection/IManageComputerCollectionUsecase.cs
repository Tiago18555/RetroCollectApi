using System.Security.Claims;
using Domain;
using Application.UseCases.UserCollectionOperations.Shared;

namespace Application.UseCases.UserCollectionOperations.ManageComputerCollection
{
    public interface IManageComputerCollectionUsecase
    {
        public Task<ResponseModel> AddComputer(AddItemRequestModel item, ClaimsPrincipal user);
        public ResponseModel DeleteComputer(Guid id, ClaimsPrincipal user);
        public Task<ResponseModel> UpdateComputer(UpdateComputerRequestModel item, ClaimsPrincipal user);
        public Task<ResponseModel> GetAllComputersByUser(ClaimsPrincipal user);

    }
}
