using System.ComponentModel.DataAnnotations;

namespace RetroCollect.Models
{
    public class UserComputer
    {
        [Key]
        public Guid UserComputerId { get; set; }
        public Guid UserId { get; set; }
        public Guid ComputerId { get; set; }

        public User User { get; set; }
        public Computer Computer { get; set; }
    }
}
