using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("")]
public class HomeController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {
        try
        {
            return View();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("Plan")]
    public IActionResult Plan()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasUserRights())
                return RedirectToAction(nameof(Index));
            return View();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("Timetable")]
    public IActionResult Timetable()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasUserRights())
                return RedirectToAction(nameof(Index));
            return View();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}