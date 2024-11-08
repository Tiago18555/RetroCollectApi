using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("console")]
public class Console
{
    [Column("console_id")]
    [Key]
    public int ConsoleId { get; set; }

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

    [Column("is_portable")]
    public bool IsPortable { get; set; }
    public IEnumerable<ConsoleCollectionItem> UserConsoles { get; set; }
}
