namespace Inventory.Api.DTOs;

/// <summary>
/// DTO за филтриране и пагиниране на продукти.
/// Замества множеството positional параметри в GetAllAsync.
/// </summary>
public class ProductQueryDto
{
    /// <summary>
    /// Търсене по име или доставчик.
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Минимално количество за филтриране.
    /// </summary>
    public int? MinQuantity { get; set; }

    /// <summary>
    /// Доставчик за филтриране.
    /// </summary>
    public string? Supplier { get; set; }

    /// <summary>
    /// Максимална цена за филтриране.
    /// </summary>
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// Номер на страница (1-based).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Брой елементи на страница.
    /// </summary>
    public int PageSize { get; set; } = 20;
}
