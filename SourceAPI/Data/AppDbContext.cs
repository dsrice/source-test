using Microsoft.EntityFrameworkCore;
using SourceAPI.Models;

namespace SourceAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasPrecision(18, 2);

            // 監査フィールドの設定
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
            entity.Property(e => e.DeletedAt);

            // グローバルクエリフィルター: 削除されていないデータのみを取得
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // シードデータ
        var seedDate = new DateTime(2025, 12, 7, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Price = 999.99m, CreatedAt = seedDate, UpdatedAt = seedDate, IsDeleted = false },
            new Product { Id = 2, Name = "Mouse", Price = 29.99m, CreatedAt = seedDate, UpdatedAt = seedDate, IsDeleted = false },
            new Product { Id = 3, Name = "Keyboard", Price = 79.99m, CreatedAt = seedDate, UpdatedAt = seedDate, IsDeleted = false }
        );
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Product>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.IsDeleted = false;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}