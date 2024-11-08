using Domain;
using System.Security.Claims;

namespace Application.UseCases.CollectionOperations.ManageGameCollection;


public interface IManageGameCollectionUsecase
{
    public Task<ResponseModel> AddGame(AddGameRequestModel item, ClaimsPrincipal user);
    public Task<ResponseModel> DeleteGame(Guid id, ClaimsPrincipal user);
    public Task<ResponseModel> UpdateGame(UpdateGameRequestModel item, ClaimsPrincipal user);
    public Task<ResponseModel> GetAllGamesByUser(ClaimsPrincipal user);
}
