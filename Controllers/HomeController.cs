using Microsoft.AspNetCore.Mvc;

namespace BCSH2BDAS2.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        public IActionResult Index()
        {
            return View();
        }
    }
}
