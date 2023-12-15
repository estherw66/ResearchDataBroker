using Microsoft.EntityFrameworkCore;
using ResearchDataBroker.Models;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

    public DbSet<FileModel> Files { get; set; }
    public DbSet<ItemModel?> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileModel>()
            .ToTable("files");
        modelBuilder.Entity<ItemModel>()
            .ToTable("items");

        modelBuilder.Entity<FileModel>()
            .HasMany(f => f.Items)
            .WithMany(i => i.Files)
            .UsingEntity(j => j.ToTable("item_files"));
    }
}