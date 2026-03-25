using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

/// <summary>
/// MVC Controller за уеб интерфейса на инвентарната система.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Главна страница - показва уеб интерфейса за управление на инвентара.
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Health check endpoint за автоматични проверки.
    /// </summary>
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok("Healthy");
    }
}
