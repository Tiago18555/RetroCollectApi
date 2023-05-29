using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageComputerCollection
{
    public interface IManageComputerCollectionService
    {
        public Task<ResponseModel> AddComputer(AddItemRequestModel item);
        public ResponseModel DeleteComputer(Guid id);
        public Task<ResponseModel> UpdateGame(UpdateComputerRequestModel item);
    }
}
