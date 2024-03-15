using RetroCollect.Models;
using RetroCollectApi.Application.Shared;
using RetroCollectApi.CrossCutting.Enums.ForModels;
using System.Text.Json.Serialization;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageConsoleCollection
{
    public class AddConsoleResponseModel
    {
        public Guid UserConsoleId { get; set; }

        private Condition Condition { get; set; }
        public string condition => Enum.GetName(typeof(Condition), Condition);

        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }

        private OwnershipStatus OwnershipStatus { get; set; }
        public string ownership_status => Enum.GetName(typeof(OwnershipStatus), OwnershipStatus);
        public InternalUser User { get; set; }
    }
}
