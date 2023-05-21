using RetroCollect.CrossCutting.Enums;
using System.ComponentModel.DataAnnotations;

namespace RetroCollect.Models
{
    public class UserConsole
    {
        [Key]
        public Guid UserConsoleId { get; set; }

        public Condition Condition { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }
        public OwnershipStatus OwnershipStatus { get; set; }

        public Guid UserId { get; set; }
        public Guid ConsoleId { get; set; }
        public User User { get; set; }
        public Console Console { get; set; }
    }
}
