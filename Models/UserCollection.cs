using RetroCollect.CrossCutting.Enums;
using System.ComponentModel.DataAnnotations;

namespace RetroCollect.Models
{
    public class UserCollection
    {
        [Key]
        public int UserCollectionId { get; set; } 
        public int UserId { get; set; }
        public int GameId { get; set; }
        public Condition Condition { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }
        public OwnershipStatus OwnershipStatus { get; set; }
        public User User { get; set; }
        public Game Game { get; set; }
    }
}
