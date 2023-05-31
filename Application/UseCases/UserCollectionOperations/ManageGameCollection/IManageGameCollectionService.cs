using RetroCollectApi.CrossCutting;
using RetroCollectApi.Repositories;
using System.Security.Claims;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems
{

    public interface IManageGameCollectionService
    {
        public Task<ResponseModel> AddGame(AddGameRequestModel item, ClaimsPrincipal user);
        public ResponseModel DeleteGame(Guid id, ClaimsPrincipal user);
        public Task<ResponseModel> UpdateGame(UpdateGameRequestModel item, ClaimsPrincipal user);
        public Task<ResponseModel> GetAllGamesByUser(Guid userId, ClaimsPrincipal user);
    }
}
