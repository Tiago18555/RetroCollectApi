using CrossCutting.Validations;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.UserCollectionOperations.ManageGameCollection;

public class AddGameRequestModel
{
    [Required]
    public int Game_id { get; set; }

    [Required]
    public int Platform_id { get; set; }

    [Required]
    public bool PlatformIsComputer { get; set; }

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
