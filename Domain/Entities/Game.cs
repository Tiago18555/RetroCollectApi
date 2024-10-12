using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Game
{
    [Key]
    public int GameId { get; set; }

    [MinLength(3)]
    [MaxLength(255)]
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public List<string> Genres { get; set; }

    public string Description { get; set; }
    public string Summary { get; set; }
    public string ImageUrl { get; set; }
    public IEnumerable<UserCollection> UserCollections { get; set; }
    public IEnumerable<Rating> Ratings { get; set; }
}
