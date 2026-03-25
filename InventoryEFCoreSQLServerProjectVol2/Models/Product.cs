namespace Inventory.Api.Models;

/// <summary>
/// Домейн модел: продукт в инвентарната система.
/// EF Core го мапира към таблица `Products`.
/// </summary>
public class Product
{
    /// <summary>Уникален идентификатор на продукта.</summary>
    public int Id { get; set; }

    /// <summary>Име на продукта (задължително).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Налично количество в инвентара.</summary>
    public int Quantity { get; set; }

    /// <summary>Цена на единица (decimal с 2 знака след десетичната в DB).</summary>
    public decimal Price { get; set; }

    /// <summary>Доставчик на продукта (задължително).</summary>
    public string Supplier { get; set; } = string.Empty;

    /// <summary>UTC дата/час на създаването.</summary>
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
}
