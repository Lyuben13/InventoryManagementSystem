namespace Inventory.Api.DTOs;

/// <summary>
/// DTO за отчет за инвентар.
/// Включва общи метрики и списък с продуктите с ниска наличност.
/// </summary>
public class InventoryReportDto
{
    /// <summary>Брой различни продукти.</summary>
    public int TotalProducts { get; set; }

    /// <summary>Общо количество единици (сума от Quantity).</summary>
    public int TotalUnits { get; set; }

    /// <summary>Оценка на стойността на инвентара (сума Quantity * Price).</summary>
    public decimal TotalInventoryValue { get; set; }

    /// <summary>Брой артикули в ниска наличност.</summary>
    public int LowStockProductsCount { get; set; }

    /// <summary>UTC момент на генериране на отчета.</summary>
    public DateTime GeneratedOnUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Продукти с `Quantity <= lowStockThreshold`.
    /// </summary>
    public List<ProductResponseDto> LowStockProducts { get; set; } = new();
}
