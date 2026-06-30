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
    public DbSet<CharacterStatistic> Statistics { get; set; }
    public DbSet<CharacterInventory> Inventory { get; set; }
    public DbSet<CharacterEquipment> Equipement { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemInstance> ItemInstances { get; set; }
    public DbSet<ItemStat> ItemStats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(a => a.Username).HasMaxLength(50);
            entity.HasIndex(a => a.Username).IsUnique();

            entity.Property(a => a.Email).HasMaxLength(150);
            entity.HasIndex(a => a.Email).IsUnique();
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.Property(c => c.Name).HasMaxLength(24);
            entity.HasIndex(c => c.Name).IsUnique();
        });

        modelBuilder.Entity<CharacterStatistic>()
            .HasIndex(s => new { s.CharacterId, s.Type })
            .IsUnique();
    }
}