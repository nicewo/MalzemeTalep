using Microsoft.EntityFrameworkCore;
using MalzemeTalep.Models;

namespace MalzemeTalep.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<ProjectRecord> Projects => Set<ProjectRecord>();
    public DbSet<MaterialRequest> Requests => Set<MaterialRequest>();
    public DbSet<RequestItem> RequestItems => Set<RequestItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Material>().HasIndex(x => x.ModelCode);
        modelBuilder.Entity<ProjectRecord>().HasIndex(x => x.Name);
        modelBuilder.Entity<RequestItem>().HasOne(x => x.Request).WithMany(x => x.Items).HasForeignKey(x => x.RequestId);
        modelBuilder.Entity<RequestItem>().HasOne(x => x.Material).WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.SetNull);
    }
}
