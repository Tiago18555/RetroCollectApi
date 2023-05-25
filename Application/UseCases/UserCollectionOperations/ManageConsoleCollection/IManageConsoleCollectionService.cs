using RetroCollectApi.Application.UseCases.UserCollectionOperations.Shared;
using RetroCollectApi.CrossCutting;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageConsoleCollection
{
    public interface IManageConsoleCollectionService
    {
        public Task<ResponseModel> AddConsole(AddItemRequestModel item);
        public Task<ResponseModel> DeleteConsole();

        public Task<ResponseModel> UpdateGame(UpdateConsoleRequestModel item);
    }
}
