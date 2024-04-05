using CrossCutting;
using System.Security.Claims;

namespace Application.UseCases.GameOperations.ManageRating
{
    public interface IManageRatingService
    {
        ResponseModel EditRating(EditRatingRequestModel requestBody, ClaimsPrincipal requestToken);
        ResponseModel RemoveRating(Guid ratingId, ClaimsPrincipal requestToken);
        Task<ResponseModel> GetAllRatingsByUser(ClaimsPrincipal requestToken,int pageNumber, int pageSize);
        Task<ResponseModel> GetAllRatingsByGame(int gameId, int pageNumber, int pageSize);
    }
}
