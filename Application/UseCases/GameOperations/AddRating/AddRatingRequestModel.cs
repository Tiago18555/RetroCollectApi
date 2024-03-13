using System.ComponentModel.DataAnnotations;

namespace RetroCollectApi.Application.UseCases.GameOperations.AddRating
{
    public class AddRatingRequestModel
    {
        [Required(ErrorMessage = "Field \"user_id\" is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Field \"game_id\" is required")]
        public int GameId { get; set; }

        [Range(0, 100)]
        public int RatingValue { get; set; }

        [MinLength(5)]
        public string Review { get; set; }
    }
}
