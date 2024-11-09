using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("users")]
public class User
{
    [Column("user_id")]
    [Key]
    public Guid UserId { get; set; }

    [Column("username")]
    [Required]
    [MinLength(3)]
    [MaxLength(64)]
    public string Username { get; set; }

    [Column("password")]
    public string Password { get; set; }

    [Column("email")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }

    [Column("verified_at")]
    public DateTime VerifiedAt { get; set; }

    [Column("first_name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string FirstName { get; set; }

    [Column("last_name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string LastName { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<ConsoleCollectionItem> UserConsoles { get; set; }
    public IEnumerable<ComputerCollectionItem> UserComputers { get; set; }
    public IEnumerable<GameCollectionItem> UserCollections { get; set; }
    public IEnumerable<Rating> Ratings { get; set; }
    public IEnumerable<Wishlist> Wishlists { get; set; }
}
