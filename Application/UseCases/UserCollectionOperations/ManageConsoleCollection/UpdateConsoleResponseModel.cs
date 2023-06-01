using RetroCollect.Models;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using System.Text.Json.Serialization;
using Console = RetroCollect.Models.Console;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageConsoleCollection
{
    public class UpdateConsoleResponseModel
    {
        public Guid UserCollectionId { get; set; }

        private Condition Condition { get; set; }
        public string condition => Enum.GetName(typeof(Condition), Condition);

        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }

        private OwnershipStatus OwnershipStatus { get; set; }
        public string ownership_status => Enum.GetName(typeof(OwnershipStatus), OwnershipStatus);

        public Guid UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public string UserName => User == null ? null : User.Username;

        public int ConsoleId { get; set; }
        [JsonIgnore]
        public Console Console { get; set; }
        public string ConsoleName => Console == null ? null : Console.Name;
    }
}
