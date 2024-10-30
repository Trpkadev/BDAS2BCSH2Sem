using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;

namespace BCSH2BDAS2.Controllers;

public class HomeController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    public IActionResult Index()
    {
        ViewData["LoggedUser"] = LoggedUser;
        return View();
    }
}