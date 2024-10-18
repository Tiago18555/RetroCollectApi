using Domain;
using System.Security.Claims;

namespace Application.UseCases.UserWishlistOperations;


public interface IWishlistUsecase
{
    Task<ResponseModel> Add(AddToUserWishlistRequestModel RequestBody, ClaimsPrincipal RequestToken);
    Task<ResponseModel> Remove(int game_id, ClaimsPrincipal RequestToken);
    Task<ResponseModel> GetAllByUser(ClaimsPrincipal RequestToken, int limit, int page);
    Task<ResponseModel> GetAllByGame(int gameId, int limit, int page);
}
