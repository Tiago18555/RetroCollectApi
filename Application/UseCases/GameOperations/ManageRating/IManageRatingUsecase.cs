using Domain;
using System.Security.Claims;

namespace Application.UseCases.GameOperations.ManageRating;

public interface IManageRatingUsecase
{
    Task<ResponseModel> EditRating(EditRatingRequestModel requestBody, ClaimsPrincipal requestToken);
    Task<ResponseModel> RemoveRating(Guid ratingId, ClaimsPrincipal requestToken);
    Task<ResponseModel> GetAllRatingsByUser(ClaimsPrincipal requestToken,int pageSize, int pageNumber);
    Task<ResponseModel> GetAllRatingsByGame(int gameId, int pageSize, int papageNumber);
}
