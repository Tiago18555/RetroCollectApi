using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.GameOperations.AddRating;

public class AddRatingRequestModel
{
    [JsonPropertyName("game_id")]
    [Required(ErrorMessage = "Field \"game_id\" is required")]
    public int GameId { get; set; }

    [JsonPropertyName("rating_value")]
    [Range(0, 100)]
    public int RatingValue { get; set; }

    [JsonPropertyName("review")]
    [MinLength(5)]
    public string Review { get; set; }
}
