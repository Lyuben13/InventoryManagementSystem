using Inventory.Api.DTOs;
using Inventory.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Inventory.Api.Controllers;

/// <summary>
/// REST endpoints за управление на продукти (CRUD + report).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // Service layer абстрахира бизнес логиката от HTTP контролера.
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Връща пагиниран списък от продукти.
    /// Ако `search` е подаден, филтрираме по `Name` и `Supplier`.
    /// Ако `minQuantity` е подаден, връщаме само продукти с количество >= стойността.
    /// Ако `supplier` е подаден, филтрираме по точен доставчик.
    /// Ако `maxPrice` е подаден, филтрираме до максимална цена.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<ProductResponseDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] int? minQuantity,
        [FromQuery] string? supplier,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page < 1)
        {
            return BadRequest("page трябва да е 1 или по-голямо число.");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest("pageSize трябва да е между 1 и 100.");
        }

        if (minQuantity.HasValue && minQuantity.Value < 0)
        {
            return BadRequest("minQuantity трябва да е 0 или положително число.");
        }

        if (maxPrice.HasValue && maxPrice.Value < 0)
        {
            return BadRequest("maxPrice трябва да е 0 или положително число.");
        }

        var query = new ProductQueryDto
        {
            Search = search,
            MinQuantity = minQuantity,
            Supplier = supplier,
            MaxPrice = maxPrice,
            Page = page,
            PageSize = pageSize
        };

        var result = await _productService.GetPagedAsync(query);
        
        _logger.LogInformation("Върнати са {Count} продукта от общо {TotalCount} (страница {Page}/{PageSize})", 
            result.Items.Count, result.TotalCount, page, pageSize);
        
        return Ok(result);
    }

    /// <summary>
    /// Връща продукт по неговия идентификатор.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product is null) return NotFound();

        return Ok(product);
    }

    /// <summary>
    /// Създава нов продукт.
    /// DTO validation се изпълнява автоматично от `[ApiController]` и атрибутите в DTO.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(CreateProductDto dto)
    {
        _logger.LogInformation("Опит за създаване на продукт: {ProductName} от {Supplier}", dto.Name, dto.Supplier);
        
        var created = await _productService.CreateAsync(dto);

        // Връщаме 201 + линк към ресурса (GET /api/products/{id}).
        _logger.LogInformation("Продуктът е създаден успешно с ID: {ProductId}", created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновява съществуващ продукт.
    /// Ако продуктът не съществува, отговаря с 404.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        var updated = await _productService.UpdateAsync(id, dto);
        if (!updated) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Изтрива продукт по ID.
    /// Ако продуктът не съществува, отговаря с 404.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Генерира отчет за наличностите.
    /// Продуктите с `Quantity` <= `lowStockThreshold` се включват в списъка `LowStockProducts`.
    /// </summary>
    [HttpGet("report")]
    public async Task<ActionResult<InventoryReportDto>> GetReport([FromQuery] int lowStockThreshold = 5)
    {
        if (lowStockThreshold < 0)
        {
            return BadRequest("lowStockThreshold трябва да е 0 или положително число.");
        }

        var report = await _productService.GetReportAsync(lowStockThreshold);
        return Ok(report);
    }
}
