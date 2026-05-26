using Microsoft.EntityFrameworkCore;
using Cataloguer.Models;

namespace Cataloguer.data;

// Creating my own database session (AppDbContext), that inherits from the EF Core base class (DbContext)
public class AppDbContext : DbContext
{
    // takes the EF Core configuration object (options) and passes this to the Db context so it can connect to the database
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    // sets database tables for relevant datasets
    public DbSet<User> Users => Set<User>();
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<CardsList> Lists => Set<CardsList>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // relationships go here
        // User → Cards (1-to-many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Cards)
            .WithOne(c => c.User)
            // .HasForeignKey(t => t.UserUserId)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // User → Lists (1-to-many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Lists)
            .WithOne(l => l.User)
            // .HasForeignKey(l => l.UserUserId)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CardsList>()
            .HasKey(c => c.ListId);
    }
}