using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;
using Console = RetroCollect.Models.Console;

namespace RetroCollect.Data
{
    public partial class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Console> Consoles { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserCollection> UserCollections { get; set; }
        public DbSet<UserComputer> UserComputers { get; set; }
        public DbSet<UserConsole> UserConsoles { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DataContext(DbContextOptions<DataContext> opt) : base(opt) { }
    }
}
