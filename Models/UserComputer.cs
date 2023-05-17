using System.ComponentModel.DataAnnotations;

namespace RetroCollect.Models
{
    public class UserComputer
    {
        [Key]
        public int UserComputerId { get; set; }
        public int UserId { get; set; }
        public int ComputerId { get; set; }

        public User User { get; set; }
        public Computer Computer { get; set; }
    }
}
