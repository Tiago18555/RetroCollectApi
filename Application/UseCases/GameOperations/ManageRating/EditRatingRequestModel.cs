using System.ComponentModel.DataAnnotations;

namespace RetroCollectApi.Application.UseCases.GameOperations.ManageRating
{
    public class EditRatingRequestModel
    {
        [Required]
        public Guid RatingId { get; set; }

        [Range(0, 100)]
        public int RatingValue { get; set; }

        [MinLength(5)]
        public string Review { get; set; }
    }
}
