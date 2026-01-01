using Microsoft.EntityFrameworkCore;
using SourceAPI.Models.DB;
using SourceAPI.Interfaces;

namespace SourceAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // IAuditableEntityを実装するすべてのエンティティに監査フィールドとクエリフィルターを設定
        ConfigureAuditableEntity<Product>(modelBuilder);
        ConfigureAuditableEntity<User>(modelBuilder);

        // Product固有の設定
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasPrecision(18, 2);
        });

        // User固有の設定
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);

            // インデックス
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // シードデータ
        var seedDate = new DateTime(2025, 12, 7, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Price = 999.99m, CreatedAt = seedDate, UpdatedAt = seedDate, IsDeleted = false },
            new Product { Id = 2, Name = "Mouse", Price = 29.99m, CreatedAt = seedDate, UpdatedAt = seedDate, IsDeleted = false },
            new Product { Id = 3, Name = "Keyboard", Price = 79.99m, CreatedAt = seedDate, UpdatedAt = seedDate, IsDeleted = false }
        );

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Email = "admin@example.com", PasswordHash = "dummy_hash", FirstName = "Admin", LastName = "User", CreatedAt = seedDate, UpdatedAt = seedDate, IsDeleted = false }
        );
    }

    /// <summary>
    /// IAuditableEntityを実装するエンティティの共通設定
    /// </summary>
    private void ConfigureAuditableEntity<TEntity>(ModelBuilder modelBuilder) where TEntity : class, IAuditableEntity
    {
        modelBuilder.Entity<TEntity>(entity =>
        {
            // 監査フィールドの設定
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
            entity.Property(e => e.DeletedAt);

            // グローバルクエリフィルター: 削除されていないデータのみを取得
            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // IAuditableEntityを実装するすべてのエンティティの監査フィールドを自動設定
        var entries = ChangeTracker.Entries<IAuditableEntity>();

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