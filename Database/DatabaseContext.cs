using Microsoft.EntityFrameworkCore;
using Technologies.Models;

namespace Technologies.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<BotanicGarden> BotanicGardens { get; set; }

    public DbSet<Plant> Plants { get; set; }

    public DbSet<BotanicGardenPlant> GardenPlants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BotanicGardenPlant>()
            .HasOne(gp => gp.Garden)
            .WithMany(bg => bg.GardenPlants)
            .HasForeignKey(gp => gp.GardenId);

        modelBuilder.Entity<BotanicGardenPlant>()
            .HasOne(gp => gp.Plant)
            .WithMany(p => p.GardenPlants)
            .HasForeignKey(gp => gp.PlantId);
    }
}