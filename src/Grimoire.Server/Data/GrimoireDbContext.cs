using Grimoire.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Grimoire.Server.Data;

public class GrimoireDbContext : DbContext
{
    public GrimoireDbContext(DbContextOptions<GrimoireDbContext> options)
        : base(options)
    {
    }

    public DbSet<GameEntity> Games => Set<GameEntity>();
    public DbSet<EmulatorEntity> Emulators => Set<EmulatorEntity>();
    public DbSet<DlcEntity> Dlcs => Set<DlcEntity>();
    public DbSet<UpdateEntity> Updates => Set<UpdateEntity>();
    public DbSet<FirmwareEntity> Firmwares => Set<FirmwareEntity>();
    public DbSet<EmulatorBinaryEntity> EmulatorBinaries => Set<EmulatorBinaryEntity>();
    public DbSet<BiosEntity> BiosFiles => Set<BiosEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.FilePath).IsRequired();
            entity.Property(e => e.Platform).HasConversion<string>();
            entity.HasIndex(e => e.Platform);
            entity.HasIndex(e => e.Title);
        });

        modelBuilder.Entity<EmulatorEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Platform).HasConversion<string>();
            entity.HasIndex(e => e.Platform).IsUnique();
        });

        modelBuilder.Entity<EmulatorBinaryEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RuntimeId).IsRequired().HasMaxLength(20);
            entity.Property(e => e.FilePath).IsRequired();
            entity.HasOne(e => e.Emulator).WithMany(e => e.Binaries).HasForeignKey(e => e.EmulatorId);
            entity.HasIndex(e => new { e.EmulatorId, e.RuntimeId }).IsUnique();
        });

        modelBuilder.Entity<DlcEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.HasOne(e => e.Game).WithMany(g => g.Dlcs).HasForeignKey(e => e.GameId);
        });

        modelBuilder.Entity<UpdateEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.HasOne(e => e.Game).WithMany(g => g.Updates).HasForeignKey(e => e.GameId);
        });

        modelBuilder.Entity<FirmwareEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Platform).HasConversion<string>();
        });

        modelBuilder.Entity<BiosEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Platform).HasConversion<string>();
        });
    }
}
