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

        public int GameId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        public string Username => User != null ? User.Username : null;
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Game Game { get; set; }
        public string GameTitle => Game != null ? Game.Title : null;
    }
}
