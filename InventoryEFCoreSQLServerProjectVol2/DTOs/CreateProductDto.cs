using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.DTOs;

/// <summary>
/// DTO за заявка при създаване на продукт.
/// Използва data-annotations, за да се валидират входните данни автоматично.
/// </summary>
public class CreateProductDto
{
    /// <summary>Име на продукта (1..100 символа).</summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Начално количество (0 и нагоре).</summary>
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    /// <summary>Цена на единица (0 и нагоре).</summary>
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal Price { get; set; }

    /// <summary>Доставчик (1..100 символа).</summary>
    [Required]
    [StringLength(100)]
    public string Supplier { get; set; } = string.Empty;
}
