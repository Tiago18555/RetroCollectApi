using RetroCollectApi.CrossCutting;
using RetroCollectApi.Repositories;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems
{

    public interface IManageGameCollectionService
    {
        public Task<ResponseModel> AddGame(AddGameRequestModel item);
        public ResponseModel DeleteGame(Guid id);
        public Task<ResponseModel> UpdateGame(UpdateGameRequestModel item);
    }
}
