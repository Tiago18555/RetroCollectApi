using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("games")]
public class Game
{
    [Column("game_id")]
    [Key]
    public int GameId { get; set; }

    [Column("title")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Title { get; set; }

    [Column("release_year")]
    public int ReleaseYear { get; set; }

    [Column("genres")]
    public List<string> Genres { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("summary")]
    public string Summary { get; set; }

    [Column("image_url")]
    public string ImageUrl { get; set; }
    public IEnumerable<GameCollectionItem> UserCollections { get; set; }
    public IEnumerable<Rating> Ratings { get; set; }
}
