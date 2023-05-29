using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using RetroCollectApi.CrossCutting;
using System.Security.Claims;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageComputerCollection
{
    public interface IManageComputerCollectionService
    {
        public Task<ResponseModel> AddComputer(AddItemRequestModel item, ClaimsPrincipal user);
        public ResponseModel DeleteComputer(Guid id, ClaimsPrincipal user);
        public Task<ResponseModel> UpdateComputer(UpdateComputerRequestModel item, ClaimsPrincipal user);
    }
}
