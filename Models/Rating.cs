using System.ComponentModel.DataAnnotations;

namespace RetroCollect.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public int RatingValue { get; set; }
        public string Review { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
