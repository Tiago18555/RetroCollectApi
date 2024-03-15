using RetroCollectApi.Application.Shared;
using RetroCollectApi.CrossCutting.Enums.ForModels;

namespace RetroCollectApi.Application.UseCases.UserCollectionOperations.ManageComputerCollection
{
    public class GetAllComputersByUserResponseModel
    {
        public Guid UserCollectionId { get; set; }

        private Condition Condition { get; set; }
        public string condition => Enum.GetName(typeof(Condition), Condition);

        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }

        private OwnershipStatus OwnershipStatus { get; set; }
        public string ownership_status => Enum.GetName(typeof(OwnershipStatus), OwnershipStatus);
        public InternalComputer Computer { get; set; }
        public int ConsoleId { get; set; }
    }
}
