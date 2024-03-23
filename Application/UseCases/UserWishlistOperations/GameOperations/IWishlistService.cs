using CrossCutting;
using System.Security.Claims;

namespace Application.UseCases.UserWishlistOperations.GameOperations
{

    public interface IWishlistService
    {
        ResponseModel Add(AddToUserWishlistRequestModel RequestBody, ClaimsPrincipal RequestToken);
        ResponseModel Remove(int game_id, ClaimsPrincipal RequestToken);
    }
}
