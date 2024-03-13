using RetroCollectApi.CrossCutting;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RetroCollectApi.Application.UseCases.GameOperations.AddRating
{
    public interface IAddRatingService
    {
        ResponseModel AddRating(AddRatingRequestModel requestBody, ClaimsPrincipal requestToken);
    }
}
