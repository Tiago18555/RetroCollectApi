using CrossCutting.Validations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.UserCollectionOperations.ManageGameCollection;

public class UpdateGameRequestModel
{
    [Required]
    public Guid UserCollectionId { get; set; }

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
