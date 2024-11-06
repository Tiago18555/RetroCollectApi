using Domain;
using System.Security.Claims;

namespace Application.UseCases.RatingOperations.AddRating;

public interface IAddRatingUsecase
{
    Task<ResponseModel> AddRating(AddRatingRequestModel requestBody, ClaimsPrincipal requestToken);
}
