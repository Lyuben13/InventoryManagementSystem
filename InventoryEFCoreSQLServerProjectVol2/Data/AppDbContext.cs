using Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Data;

/// <summary>
/// EF Core DbContext, който описва моделите и seed-ва начални данни.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSet-ът дава "точка" за CRUD заявки към таблица/колекцията Products.
    public DbSet<Product> Products => Set<Product>();

    // Fluent API конфигурация (валидации, типове на колони) + seed данни.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            // Ограничения върху полета на ниво база данни/модел.
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Supplier).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Price).HasColumnType("decimal(18,2)");

            // Database индекси за performance
            entity.HasIndex(p => p.Name).HasDatabaseName("IX_Products_Name");
            entity.HasIndex(p => p.Supplier).HasDatabaseName("IX_Products_Supplier");
            entity.HasIndex(p => p.Quantity).HasDatabaseName("IX_Products_Quantity");
            entity.HasIndex(p => p.Price).HasDatabaseName("IX_Products_Price");
            entity.HasIndex(p => new { p.Name, p.Supplier }).HasDatabaseName("IX_Products_Name_Supplier");
            entity.HasIndex(p => p.CreatedOnUtc).HasDatabaseName("IX_Products_CreatedOnUtc");

            // Seed данни се вграждат чрез migration-и (EF ще ги "upsert"-ва при първото създаване).
            entity.HasData(
                new Product { Id = 1, Name = "Keyboard", Quantity = 15, Price = 49.90m, Supplier = "Tech Supply Ltd", CreatedOnUtc = new DateTime(2026, 3, 20, 9, 15, 30, DateTimeKind.Utc) },
                new Product { Id = 2, Name = "Mouse", Quantity = 30, Price = 24.50m, Supplier = "Office Gear", CreatedOnUtc = new DateTime(2026, 3, 20, 14, 42, 15, DateTimeKind.Utc) },
                new Product { Id = 3, Name = "Monitor", Quantity = 5, Price = 289.99m, Supplier = "Display House", CreatedOnUtc = new DateTime(2026, 3, 20, 16, 30, 45, DateTimeKind.Utc) }
            );
        });
    }
}
