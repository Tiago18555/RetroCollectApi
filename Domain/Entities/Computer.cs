using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("computers")]
public class Computer
{
    [Column("computer_id")]
    [Key]
    public int ComputerId { get; set; }

    [Column("name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Name { get; set; }

    [Column("description")]
    [MinLength(3)]
    [MaxLength(2048)]
    public string Description { get; set; }

    [Column("image_url")]
    [MinLength(1)]
    public string ImageUrl { get; set; }

    [Column("is_arcade")]
    public bool IsArcade { get; set; }
    public IEnumerable<ComputerCollectionItem> UserComputers { get; set; }
}
