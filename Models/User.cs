using System.ComponentModel.DataAnnotations;

namespace RetroCollect.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string Username { get; set; }
        public string Password { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string Email { get; set; }
        public DateTime VerifiedAt { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [MinLength(3)]
        [MaxLength(255)]
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<UserConsole> UserConsoles { get; set; }
        public IEnumerable<UserComputer> UserComputers { get; set; }
        public IEnumerable<UserCollection> UserCollections { get; set; }
        public IEnumerable<Rating> Ratings { get; set; }
    }
}
