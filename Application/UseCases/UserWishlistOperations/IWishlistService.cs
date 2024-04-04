using CrossCutting;
using System.Security.Claims;

namespace Application.UseCases.UserWishlistOperations
{

    public interface IWishlistService
    {
        ResponseModel Add(AddToUserWishlistRequestModel RequestBody, ClaimsPrincipal RequestToken);
        ResponseModel Remove(int game_id, ClaimsPrincipal RequestToken);
        Task<ResponseModel> GetAllByUser(ClaimsPrincipal RequestToken);
    }
}
