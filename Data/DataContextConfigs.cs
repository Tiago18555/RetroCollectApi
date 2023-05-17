using Microsoft.EntityFrameworkCore;
using RetroCollect.Models;

namespace RetroCollect.Data
{
    public partial class DataContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Username)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();
        }
    }
}
