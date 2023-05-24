using RetroCollectApi.CrossCutting.Enums.ForModels;
using System.ComponentModel.DataAnnotations;

namespace RetroCollect.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string Title { get; set; }
        public int ConsoleId { get; set; }
        public int ComputerId { get; set; }
        public int ReleaseYear { get; set; }
        public string Genre { get; set; }

        public string Description { get; set; }
        public string Summary { get; set; }
        public string ImageUrl { get; set; }

        public Console Console { get; set; }
        public Computer Computer { get; set; }
        public ICollection<UserCollection> UserCollections { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}
