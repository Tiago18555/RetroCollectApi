using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.GameOperations.ManageRating;

public class EditRatingRequestModel
{
    [JsonPropertyName("rating_id")]
    [Required]
    public Guid RatingId { get; set; }

    [JsonPropertyName("rating_value")]
    [Range(0, 100)]
    public int RatingValue { get; set; }

    [JsonPropertyName("review")]
    [MinLength(5)]
    public string Review { get; set; }
}
