using RetroCollect.Models;
using RetroCollectApi.Application.UseCases.GameOperations.Shared;
using System.Text.Json.Serialization;

namespace RetroCollectApi.Application.UseCases.GameOperations.AddRating
{
    public class AddRatingResponseModel
    {
        public int RatingValue { get; set; }
        public string Review { get; set; }
        public DateTime CreatedAt { get; set; }
        public InternalGame Game { get; set; }
        public InternalUser User { get; set; }
    }
}
