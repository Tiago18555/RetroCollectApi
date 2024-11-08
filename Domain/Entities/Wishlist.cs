using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("wishlist")]
public class Wishlist
{
    [Column("wishlist_id")]
    [Key]
    public Guid WishlistId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("game_id")]
    public int GameId { get; set; }
    public User User { get; set; }
    public Game Game { get; set; }
}
