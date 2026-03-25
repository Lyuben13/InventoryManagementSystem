using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.DTOs;

/// <summary>
/// DTO за заявка при обновяване на продукт.
/// DTO-то се валидира автоматично (защото контролерът е `[ApiController]`).
/// </summary>
public class UpdateProductDto
{
    /// <summary>Ново име на продукта.</summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Ново количество (0 и нагоре).</summary>
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    /// <summary>Нова цена (>= 0).</summary>
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal Price { get; set; }

    /// <summary>Нов доставчик.</summary>
    [Required]
    [StringLength(100)]
    public string Supplier { get; set; } = string.Empty;
}
