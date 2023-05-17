using System.ComponentModel.DataAnnotations;

namespace RetroCollect.Models
{
    public class UserConsole
    {
        [Key]
        public int UserConsoleId { get; set; }
        public int UserId { get; set; }
        public int ConsoleId { get; set; }

        public User User { get; set; }
        public Console Console { get; set; }
    }
}
