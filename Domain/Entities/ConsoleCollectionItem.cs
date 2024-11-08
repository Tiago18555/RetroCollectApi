using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("console_collection")]
public class ConsoleCollectionItem
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

    [Column("console_id")]
    public int ConsoleId { get; set; }
    public User User { get; set; }
    public Console Console { get; set; }
}
