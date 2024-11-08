using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("rating")]
public class Rating
{
    [Column("rating_id")]
    [Key]
    public Guid RatingId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("game_id")]
    public int GameId { get; set; }

    [Column("rating_value")]
    [Range(0, 100)]
    public int RatingValue { get; set; }

    [Column("review")]
    [MinLength(5)]
    public string Review { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    public User User { get; set; }
    public Game Game { get; set; }

}
