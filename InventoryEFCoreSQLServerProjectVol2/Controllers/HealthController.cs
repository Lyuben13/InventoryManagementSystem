using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

/// <summary>
/// Health Check Controller за мониторинг на системата
/// </summary>
public class HealthController : Controller
{
    /// <summary>
    /// Главна health check страница
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }
}
