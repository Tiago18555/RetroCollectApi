using RetroCollectApi.CrossCutting;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RetroCollectApi.Application.UseCases.GameOperations.AddRating
{
    public interface IAddRatingService
    {
        Task<ResponseModel> AddRating(AddRatingRequestModel requestBody, ClaimsPrincipal requestToken);
    }
}
