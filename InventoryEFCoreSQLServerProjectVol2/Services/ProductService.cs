using Inventory.Api.DTOs;
using Inventory.Api.Models;
using Inventory.Api.Repositories;

namespace Inventory.Api.Services;

/// <summary>
/// Бизнес логика за продукти.
/// Service слойът оркестрира repository-операциите и прави мапинг между domain model и DTO.
/// </summary>
public class ProductService : IProductService
{
    // Repository, който абстрахира EF Core persistence.
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Връща пагиниран списък от продукти (DTO) с филтриране.
    /// </summary>
    public async Task<PagedResultDto<ProductResponseDto>> GetPagedAsync(ProductQueryDto query, CancellationToken cancellationToken = default)
    {
        var (products, totalCount) = await _repository.GetAllAsync(
            query.Search, query.MinQuantity, query.Supplier, query.MaxPrice, query.Page, query.PageSize);
        
        return new PagedResultDto<ProductResponseDto>
        {
            Items = products.Select(MapToDto).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Връща продукт (DTO) по ID, или `null`, ако не съществува.
    /// </summary>
    public async Task<ProductResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id);
        return product is null ? null : MapToDto(product);
    }

    /// <summary>
    /// Създава продукт от DTO, записва го в DB и връща DTO за отговор.
    /// </summary>
    public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            // Нормализираме входа, за да избегнем записи с излишни интервали.
            Name = dto.Name.Trim(),
            Quantity = dto.Quantity,
            Price = dto.Price,
            Supplier = dto.Supplier.Trim()
        };

        _repository.Add(product);
        await _repository.SaveChangesAsync();
        return MapToDto(product);
    }

    /// <summary>
    /// Обновява продукт по ID.
    /// Връща `true`, ако е намерен и обновен; иначе `false`.
    /// </summary>
    public async Task<bool> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null) return false;

        product.Name = dto.Name.Trim();
        product.Quantity = dto.Quantity;
        product.Price = dto.Price;
        product.Supplier = dto.Supplier.Trim();

        // EF Core change tracking ще открие промените автоматично
        return await _repository.SaveChangesAsync();
    }

    /// <summary>
    /// Изтрива продукт по ID.
    /// Връща `true`, ако е намерен и изтрит; иначе `false`.
    /// </summary>
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null) return false;

        _repository.Delete(product);
        return await _repository.SaveChangesAsync();
    }

    /// <summary>
    /// Генерира отчет:
    /// - общ брой продукти/единици
    /// - обща стойност (Quantity * Price)
    /// - списък от продукти с ниска наличност по зададен праг.
    /// </summary>
    public async Task<InventoryReportDto> GetReportAsync(int lowStockThreshold = 10, CancellationToken cancellationToken = default)
    {
        // Използваме специален метод за отчет, който връща всички продукти без paging
        var products = await _repository.GetAllForReportAsync();
        
        return new InventoryReportDto
        {
            TotalProducts = products.Count,
            TotalUnits = products.Sum(p => p.Quantity),
            TotalInventoryValue = products.Sum(p => p.Quantity * p.Price),
            LowStockProductsCount = products.Count(p => p.Quantity <= lowStockThreshold),
            GeneratedOnUtc = DateTime.UtcNow,
            LowStockProducts = products
                .Where(p => p.Quantity <= lowStockThreshold)
                .Select(MapToDto)
                .ToList()
        };
    }

    // Private mapping helper: конвертира domain model към DTO.
    private static ProductResponseDto MapToDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Quantity = product.Quantity,
        Price = product.Price,
        Supplier = product.Supplier,
        CreatedOnUtc = product.CreatedOnUtc
    };
}
