using Microsoft.EntityFrameworkCore;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {

    }

    public DbSet<FileModel> Files { get; set; }
    public DbSet<ItemModel> Items { get; set; }
}