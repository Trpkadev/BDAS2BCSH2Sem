using Microsoft.AspNetCore.Mvc;

namespace BCSH2BDAS2.Controllers;

public class HomeController() : Controller
{
    public IActionResult Index()
    {
        ViewData["Role"] = 0;
        return View();
    }
}