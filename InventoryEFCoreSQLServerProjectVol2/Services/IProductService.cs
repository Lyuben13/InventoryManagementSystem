using Inventory.Api.DTOs;

namespace Inventory.Api.Services;

/// <summary>
/// Контракт за бизнес операции върху продукти.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Връща пагиниран списък от продукти (DTO) с филтриране.
    /// </summary>
    Task<PagedResultDto<ProductResponseDto>> GetPagedAsync(ProductQueryDto query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Връща продукт (DTO) по ID, или `null`, ако не съществува.
    /// </summary>
    Task<ProductResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Създава продукт от DTO, записва го в DB и връща DTO за отговор.
    /// </summary>
    Task<ProductResponseDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновява продукт по ID.
    /// Връща `true`, ако е намерен и обновен; иначе `false`.
    /// </summary>
    Task<bool> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Изтрива продукт по ID.
    /// Връща `true`, ако е намерен и изтрит; иначе `false`.
    /// </summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Генерира отчет за наличности.
    /// </summary>
    Task<InventoryReportDto> GetReportAsync(int lowStockThreshold = 10, CancellationToken cancellationToken = default);
}
