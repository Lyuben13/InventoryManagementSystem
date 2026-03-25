namespace Inventory.Api.DTOs;

/// <summary>
/// DTO за отговор (read model) при продукт.
/// Включва изчислена стойност `TotalValue` като удобна агрегация.
/// </summary>
public class ProductResponseDto
{
    /// <summary>Идентификатор на продукта.</summary>
    public int Id { get; set; }

    /// <summary>Име на продукта.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Текущо количество.</summary>
    public int Quantity { get; set; }

    /// <summary>Цена на единица.</summary>
    public decimal Price { get; set; }

    /// <summary>Доставчик.</summary>
    public string Supplier { get; set; } = string.Empty;

    /// <summary>UTC момент на създаване на записа.</summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Удобна изчислена стойност: `Quantity * Price`.
    /// </summary>
    public decimal TotalValue => Quantity * Price;
}
