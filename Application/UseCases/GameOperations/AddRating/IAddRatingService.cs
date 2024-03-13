using RetroCollectApi.CrossCutting;
using System.ComponentModel.DataAnnotations;

namespace RetroCollectApi.Application.UseCases.GameOperations.AddRating
{
    public interface IAddRatingService
    {
        ResponseModel AddRating(AddRatingRequestModel request);
    }
}
