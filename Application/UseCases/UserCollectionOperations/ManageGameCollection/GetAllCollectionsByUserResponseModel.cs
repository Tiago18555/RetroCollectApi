using RetroCollect.Models;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using System.Text.Json.Serialization;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.AddItems
{
    public partial class ManageGameCollectionService
    {
        public class GetAllCollectionsByUserResponseModel
        {
            public Guid UserCollectionId { get; set; }

            private Condition Condition { get; set; }
            public string condition => Enum.GetName(typeof(Condition), Condition);

            public DateTime PurchaseDate { get; set; }
            public string Notes { get; set; }

            private OwnershipStatus OwnershipStatus { get; set; }
            public string ownership_status => Enum.GetName(typeof(OwnershipStatus), OwnershipStatus);
            public int GameId { get; set; }
            [JsonIgnore]
            public Game Game { get; set; }
            public string GameTitle => Game == null ? null : Game.Title;

            public int ComputerId { get; set; }
            public int ConsoleId { get; set; }
        }
    }
}
