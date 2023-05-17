using System.ComponentModel.DataAnnotations;

namespace RetroCollect.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string Username { get; set; }
        public string Password { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string Email { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<UserConsole> UserConsoles { get; set; }
        public ICollection<UserComputer> UserComputers { get; set; }
        public ICollection<UserCollection> UserCollections { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}
