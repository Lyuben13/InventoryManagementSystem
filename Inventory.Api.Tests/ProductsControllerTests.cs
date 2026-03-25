using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Inventory.Api.Data;
using Inventory.Api.Models;

namespace Inventory.Api.Tests;

/// <summary>
/// Integration тестове за ProductsController.
/// Използват WebApplicationFactory за пълен HTTP pipeline тест.
/// </summary>
public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ProductsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Премахваме съществуващия DbContext и добавяме InMemory
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsPagedResult()
    {
        // Arrange
        await SeedTestData();

        // Act
        var response = await _client.GetAsync("/api/products?page=1&pageSize=10");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<ProductResponseDto>>();
        
        Assert.NotNull(result);
        Assert.True(result.Items.Count > 0);
        Assert.True(result.TotalCount > 0);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
    }

    [Fact]
    public async Task Create_ValidProduct_ReturnsCreated()
    {
        // Arrange
        var product = new
        {
            Name = "Test Product",
            Quantity = 10,
            Price = 99.99m,
            Supplier = "Test Supplier"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", product);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<ProductResponseDto>();
        
        Assert.NotNull(created);
        Assert.Equal("Test Product", created.Name);
        Assert.Equal(10, created.Quantity);
        Assert.Equal(99.99m, created.Price);
        Assert.Equal("Test Supplier", created.Supplier);
    }

    [Fact]
    public async Task Create_InvalidProduct_ReturnsBadRequest()
    {
        // Arrange
        var product = new
        {
            Name = "", // невалидно - празно име
            Quantity = -1, // невалидно - отрицателно количество
            Price = -10m, // невалидно - отрицателна цена
            Supplier = "" // невалидно - празен доставчик
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", product);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        await SeedTestData();

        // Act
        var response = await _client.GetAsync("/api/products/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var product = await response.Content.ReadFromJsonAsync<ProductResponseDto>();
        
        Assert.NotNull(product);
        Assert.Equal(1, product.Id);
    }

    [Fact]
    public async Task GetById_NonExistingProduct_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/products/999");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_ExistingProduct_ReturnsNoContent()
    {
        // Arrange
        await SeedTestData();
        var update = new
        {
            Name = "Updated Product",
            Quantity = 20,
            Price = 199.99m,
            Supplier = "Updated Supplier"
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/products/1", update);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingProduct_ReturnsNoContent()
    {
        // Arrange
        await SeedTestData();

        // Act
        var response = await _client.DeleteAsync("/api/products/1");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GetReport_ReturnsInventoryReport()
    {
        // Arrange
        await SeedTestData();

        // Act
        var response = await _client.GetAsync("/api/products/report?lowStockThreshold=5");

        // Assert
        response.EnsureSuccessStatusCode();
        var report = await response.Content.ReadFromJsonAsync<InventoryReportDto>();
        
        Assert.NotNull(report);
        Assert.True(report.TotalProducts > 0);
        Assert.True(report.TotalUnits > 0);
        Assert.True(report.TotalInventoryValue > 0);
    }

    private async Task SeedTestData()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        dbContext.Products.AddRange(
            new Product { Name = "Test Product 1", Quantity = 10, Price = 50m, Supplier = "Supplier 1" },
            new Product { Name = "Test Product 2", Quantity = 5, Price = 25m, Supplier = "Supplier 2" },
            new Product { Name = "Test Product 3", Quantity = 3, Price = 75m, Supplier = "Supplier 1" }
        );
        
        await dbContext.SaveChangesAsync();
    }
}

// Helper DTOs за тестовете
public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Supplier { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
}

public class PagedResultDto<T>
{
    public List<T> Items { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

public class InventoryReportDto
{
    public int TotalProducts { get; set; }
    public int TotalUnits { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public int LowStockProductsCount { get; set; }
    public DateTime GeneratedOnUtc { get; set; }
    public List<ProductResponseDto> LowStockProducts { get; set; } = new();
}
