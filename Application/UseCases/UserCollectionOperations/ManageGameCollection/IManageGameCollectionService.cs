using CrossCutting;
using System.Security.Claims;

namespace Application.UseCases.UserCollectionOperations.AddItems
{

    public interface IManageGameCollectionService
    {
        public Task<ResponseModel> AddGame(AddGameRequestModel item, ClaimsPrincipal user);
        public ResponseModel DeleteGame(Guid id, ClaimsPrincipal user);
        public Task<ResponseModel> UpdateGame(UpdateGameRequestModel item, ClaimsPrincipal user);
        public Task<ResponseModel> GetAllGamesByUser(ClaimsPrincipal user);
    }
}
