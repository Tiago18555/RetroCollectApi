using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Rating
{
    [Key]
    public Guid RatingId { get; set; }
    public Guid UserId { get; set; }
    public int GameId { get; set; }
    public User User { get; set; }
    public Game Game { get; set; }

    [Range(0, 100)]
    public int RatingValue { get; set; }

    [MinLength(5)]
    public string Review { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
