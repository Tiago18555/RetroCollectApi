using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
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
        public int ConsoleId { get; set; }
        public User User { get; set; }
        public Console Console { get; set; }
    }
}
