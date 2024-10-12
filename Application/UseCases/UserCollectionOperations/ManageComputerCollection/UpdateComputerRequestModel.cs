using Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.UserCollectionOperations.ManageComputerCollection
{
    public class UpdateComputerRequestModel
    {
        [Required]
        public Guid User_id { get; set; }
        public int Item_id { get; set; }

        [Required]
        public Guid UserComputerId { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [CustomDate(ErrorMessage = "Invalid date format.")]
        [NotFutureDate(ErrorMessage = "Date cannot be in the future.")]
        public DateTime PurchaseDate { get; set; }

        [IsValidCondition]
        public string Condition { get; set; }

        [IsValidOwnershipStatus]
        public string OwnershipStatus { get; set; }

        public string Notes { get; set; }
    }
}
