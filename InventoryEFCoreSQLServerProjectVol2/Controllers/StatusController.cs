using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

public class StatusController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
