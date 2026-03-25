using Inventory.Api.DTOs;
using Inventory.Api.Models;
using Inventory.Api.Repositories;
using Inventory.Api.Services;

namespace Inventory.Api.Tests;

/// <summary>
/// Unit тестове за ProductService.
/// Използват in-memory fake repository, за да валидират бизнес логиката без реална база.
/// </summary>
public class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_TrimsInput_AndReturnsCreatedDto()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var dto = new CreateProductDto
        {
            Name = "  Keyboard  ",
            Quantity = 10,
            Price = 99.90m,
            Supplier = "  Tech Supplier  "
        };

        var created = await service.CreateAsync(dto);

        Assert.Equal(1, created.Id);
        Assert.Equal("Keyboard", created.Name);
        Assert.Equal("Tech Supplier", created.Supplier);
        Assert.Equal(10, created.Quantity);
        Assert.Equal(99.90m, created.Price);
        Assert.Equal(999.00m, created.TotalValue);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductExists_UpdatesAndReturnsTrue()
    {
        var repository = new FakeProductRepository(
            new Product { Id = 7, Name = "Old", Quantity = 1, Price = 1m, Supplier = "Old Supplier" });
        var service = new ProductService(repository);

        var updated = await service.UpdateAsync(7, new UpdateProductDto
        {
            Name = "  New Name ",
            Quantity = 25,
            Price = 199.99m,
            Supplier = "  New Supplier "
        });

        Assert.True(updated);
        var reloaded = await repository.GetByIdAsync(7);
        Assert.NotNull(reloaded);
        Assert.Equal("New Name", reloaded!.Name);
        Assert.Equal(25, reloaded.Quantity);
        Assert.Equal(199.99m, reloaded.Price);
        Assert.Equal("New Supplier", reloaded.Supplier);
    }

    [Fact]
    public async Task UpdateAsync_WhenMissingProduct_ReturnsFalse()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var updated = await service.UpdateAsync(404, new UpdateProductDto
        {
            Name = "Doesn't matter",
            Quantity = 1,
            Price = 1,
            Supplier = "N/A"
        });

        Assert.False(updated);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductExists_DeletesAndReturnsTrue()
    {
        var repository = new FakeProductRepository(
            new Product { Id = 3, Name = "Mouse", Quantity = 10, Price = 20m, Supplier = "Office Gear" });
        var service = new ProductService(repository);

        var deleted = await service.DeleteAsync(3);

        Assert.True(deleted);
        Assert.Null(await repository.GetByIdAsync(3));
    }

    [Fact]
    public async Task GetReportAsync_ReturnsExpectedAggregates()
    {
        var repository = new FakeProductRepository(
            new Product { Id = 1, Name = "A", Quantity = 2, Price = 10m, Supplier = "S1" },
            new Product { Id = 2, Name = "B", Quantity = 8, Price = 5m, Supplier = "S2" },
            new Product { Id = 3, Name = "C", Quantity = 1, Price = 100m, Supplier = "S3" });
        var service = new ProductService(repository);

        var report = await service.GetReportAsync(lowStockThreshold: 2);

        Assert.Equal(3, report.TotalProducts);
        Assert.Equal(11, report.TotalUnits);
        Assert.Equal(160m, report.TotalInventoryValue);
        Assert.Equal(2, report.LowStockProductsCount);
        Assert.Equal(2, report.LowStockProducts.Count);
        Assert.Contains(report.LowStockProducts, p => p.Id == 1);
        Assert.Contains(report.LowStockProducts, p => p.Id == 3);
        Assert.True(report.GeneratedOnUtc <= DateTime.UtcNow);
    }

    private sealed class FakeProductRepository : IProductRepository
    {
        private readonly List<Product> _products;
        private int _nextId;

        public FakeProductRepository(params Product[] initialProducts)
        {
            _products = initialProducts.ToList();
            _nextId = _products.Count == 0 ? 1 : _products.Max(p => p.Id) + 1;
        }

        public Task<(List<Product> products, int totalCount)> GetAllAsync(string? search, int? minQuantity, string? supplier, decimal? maxPrice, int page = 1, int pageSize = 20)
        {
            IEnumerable<Product> query = _products;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(p =>
                    p.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    p.Supplier.Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            if (minQuantity.HasValue)
            {
                query = query.Where(p => p.Quantity >= minQuantity.Value);
            }

            if (!string.IsNullOrWhiteSpace(supplier))
            {
                var supplierTerm = supplier.Trim();
                query = query.Where(p => p.Supplier.Equals(supplierTerm, StringComparison.Ordinal));
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            var totalCount = query.Count();
            var pagedProducts = query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Task.FromResult((pagedProducts, totalCount));
        }

        public Task<Product?> GetByIdAsync(int id) =>
            Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

        public Task<List<Product>> GetAllForReportAsync() =>
            Task.FromResult(_products.ToList());

        public void Add(Product product)
        {
            if (product.Id == 0)
            {
                product.Id = _nextId++;
            }

            _products.Add(product);
        }

        public void Update(Product product)
        {
            // In-memory list, no need to update since we work with references
            // This is a no-op for the fake implementation
        }

        public void Delete(Product product)
        {
            _products.Remove(product);
        }

        public Task<bool> SaveChangesAsync() => Task.FromResult(true);
    }
}
