using CrossCutting;
using System.Security.Claims;

namespace Application.UseCases.GameOperations.AddRating
{
    public interface IAddRatingService
    {
        Task<ResponseModel> AddRating(AddRatingRequestModel requestBody, ClaimsPrincipal requestToken);
    }
}
