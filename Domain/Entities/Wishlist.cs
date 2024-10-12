using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Wishlist
{
    [Key]
    public Guid WishlistId { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public int GameId { get; set; }
    public Game Game { get; set; }
}
