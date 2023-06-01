using RetroCollect.Models;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using System.Text.Json.Serialization;
using Game = RetroCollect.Models.Game;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems
{
    public class UpdateGameResponseModel
    {
        public Guid UserCollectionId { get; set; }

        private Condition Condition { get; set; }
        public string condition => Enum.GetName(typeof(Condition), Condition);

        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }

        private OwnershipStatus OwnershipStatus { get; set; }
        public string ownership_status => Enum.GetName(typeof(OwnershipStatus), OwnershipStatus);

        public Guid UserId { get; set; }
        public int GameId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        public string UserName => User == null ? null : User.Username;

        [JsonIgnore]
        public Game Game { get; set; }
        public string GameTitle => Game == null ? null : Game.Title;

        public int ComputerId { get; set; }
        public int ConsoleId { get; set; }
    }
}
