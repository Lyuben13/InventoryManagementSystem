using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

/// <summary>
/// Privacy Controller за информация за поверителност
/// </summary>
public class PrivacyController : Controller
{
    /// <summary>
    /// Privacy страница
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }
}
