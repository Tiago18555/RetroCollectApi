using RetroCollect.Models;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using System.Text.Json.Serialization;
using Console = RetroCollect.Models.Console;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageConsoleCollection
{
    public class GetAllConsolesByUserResponseModel
    {
        public Guid UserCollectionId { get; set; }

        private Condition Condition { get; set; }
        public string condition => Enum.GetName(typeof(Condition), Condition);

        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }

        private OwnershipStatus OwnershipStatus { get; set; }
        public string ownership_status => Enum.GetName(typeof(OwnershipStatus), OwnershipStatus);

        public int ConsoleId { get; set; }
        [JsonIgnore]
        public Console Console { get; set; }
        public string ConsoleName => Console == null ? null : Console.Name;
    }
}
