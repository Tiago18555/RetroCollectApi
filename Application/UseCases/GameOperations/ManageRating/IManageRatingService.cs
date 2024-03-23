using CrossCutting;
using System.Security.Claims;

namespace Application.UseCases.GameOperations.ManageRating
{
    public interface IManageRatingService
    {
        ResponseModel EditRating(EditRatingRequestModel requestBody, ClaimsPrincipal requestToken);
        ResponseModel RemoveRating(Guid ratingId, ClaimsPrincipal requestToken);
    }
}
