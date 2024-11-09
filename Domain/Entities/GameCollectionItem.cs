using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("game_collections")]
public class GameCollectionItem
{

    [Column("id")]
    [Key]
    public Guid Id { get; set; }

    [Column("condition")]
    public Condition Condition { get; set; }

    [Column("purchase_date")]
    public DateTime PurchaseDate { get; set; }

    [Column("notes")]
    public string Notes { get; set; }

    [Column("ownership_status")]
    public OwnershipStatus OwnershipStatus { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }
    
    [Column("game_id")]
    public int GameId { get; set; }

    [Column("computer_id")]
    public int ComputerId { get; set; }
    
    [Column("console_id")] 
    public int ConsoleId { get; set; }

    public User User { get; set; }
    public Game Game { get; set; }
}
