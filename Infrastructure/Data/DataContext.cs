using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Console = Domain.Entities.Console;

namespace Infrastructure.Data;

public partial class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Console> Consoles { get; set; }
    public DbSet<Computer> Computers { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<GameCollectionItem> GameCollectionItems { get; set; }
    public DbSet<ComputerCollectionItem> ComputerCollectionItems { get; set; }
    public DbSet<ConsoleCollectionItem> ConsoleCollectionItems { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DataContext(DbContextOptions<DataContext> opt) : base(opt) { }
}