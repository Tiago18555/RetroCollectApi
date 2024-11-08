using CrossCutting.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.CollectionOperations.ManageConsoleCollection;

public class UpdateConsoleRequestModel
{
    [JsonPropertyName("id")]
    [Required]
    public Guid Id { get; set; }

    [JsonPropertyName("purchase_date")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    [CustomDate(ErrorMessage = "Invalid date format.")]
    [NotFutureDate(ErrorMessage = "Date cannot be in the future.")]
    public DateTime PurchaseDate { get; set; }

    [JsonPropertyName("condition")]
    [IsValidCondition]
    public string Condition { get; set; }

    [JsonPropertyName("ownership_status")]
    [IsValidOwnershipStatus]
    public string OwnershipStatus { get; set; }

    [JsonPropertyName("notes")]
    public string Notes { get; set; }
}
