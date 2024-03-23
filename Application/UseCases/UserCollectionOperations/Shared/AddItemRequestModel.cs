using System.ComponentModel.DataAnnotations;
using CrossCutting.Validations;

namespace Application.UseCases.UserCollectionOperations.Shared
{
    public class AddItemRequestModel
    {
        [Required]
        public int Item_id { get; set; }

        [Required]
        public Guid User_id { get; set; }

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
