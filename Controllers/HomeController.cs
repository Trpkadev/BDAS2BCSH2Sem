using Microsoft.AspNetCore.Mvc;

namespace BCSH2BDAS2.Controllers;

[Route("")]
public class HomeController() : Controller
{
    [Route("")]
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}