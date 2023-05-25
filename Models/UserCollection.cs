using RetroCollectApi.CrossCutting.Enums.ForModels;
using System.ComponentModel.DataAnnotations;

namespace RetroCollect.Models
{
    public class UserCollection
    {
        [Key]
        public Guid UserCollectionId { get; set; }

        public Condition Condition { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }
        public OwnershipStatus OwnershipStatus { get; set; }

        public Guid UserId { get; set; }
        public int GameId { get; set; }
        public User User { get; set; }
        public Game Game { get; set; }

        public int ComputerId { get; set; }
        public int ConsoleId { get; set; }
    }
}
