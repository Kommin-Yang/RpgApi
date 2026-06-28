using Microsoft.EntityFrameworkCore;
using RpgApi.Models;

namespace RpgApi.Data;

public class RpgDbContext : DbContext
{
    public RpgDbContext(DbContextOptions<RpgDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Statistics> Statistics { get; set; }
    public DbSet<Inventory> Inventory { get; set; }
    public DbSet<Equipement> Equipement { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemInstance> ItemInstances { get; set; }
    public DbSet<ItemStats> ItemStats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Statistics>()
            .HasIndex(s => new { s.CharacterId, s.Type })
            .IsUnique();
    }
}