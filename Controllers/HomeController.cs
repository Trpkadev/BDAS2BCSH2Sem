using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    public async Task<IActionResult> Plan()
    {
        try
        {
            if (ActingUser == null)
                return RedirectToAction(nameof(Index));
            return View(await _context.GetZastavkyAsync());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Route("Plan")]
    public async Task<ActionResult> Plan(string? od, string? _do, string? cas)
    {
        // TODO Použít funkci v DB a zobrazit výsledek
        var zastavky = await _context.GetZastavkyAsync();
        ViewBag.Zastavky = new SelectList(zastavky);
        return View();
    }

    [HttpGet]
    [Route("Timetable")]
    public async Task<ActionResult> Timetable(string? encryptedId)
    {
        if (encryptedId == null)
        {
            var linky = await _context.GetLinkyAsync();
            return View(linky);
        }
        // TODO načíst víc než jen linku
        int linkaId = OurCryptography.Instance.DecryptId(encryptedId);
        var linka = await _context.GetLinkyByIdAsync(linkaId);
        return View(linka);
    }
}