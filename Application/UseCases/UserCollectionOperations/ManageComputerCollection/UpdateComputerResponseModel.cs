using RetroCollect.Models;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using System.Text.Json.Serialization;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageComputerCollection
{
    public class UpdateComputerResponseModel
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

        public int ComputerId { get; set; }
        [JsonIgnore]
        public Computer Computer { get; set; }
        public string ComputerName => Computer == null ? null : Computer.Name;
    }
}
