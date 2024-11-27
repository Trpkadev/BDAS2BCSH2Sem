using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        var vm = new TimetableViewModel()
        {
            Routes = await _context.GetLinkyAsync() ?? [],
        };
        if (encryptedId == null)
        {
            return View(vm);
        }

        int linkaId = OurCryptography.Instance.DecryptId(encryptedId);
        var linka = await _context.GetLinkyByIdAsync(linkaId);
        if (linka == null)
        {
            return StatusCode(404);
        }

        var jr = await _context.JizdniRady.FromSql($"SELECT JR.*, Z.NAZEV AS ZastavkaNazev FROM JIZDNI_RADY JR JOIN SPOJE S ON S.ID_SPOJ = JR.ID_SPOJ JOIN ST69642.ZASTAVKY Z ON JR.ID_ZASTAVKA = Z.ID_ZASTAVKA WHERE ID_LINKA = {linkaId}").ToListAsync();
        var zastavky = jr.Select(jr => jr.ZastavkaNazev).Distinct().ToList();

        vm.Timetable = [];
        vm.CisloLinky = linka.Cislo;

        foreach (var zastavka in zastavky)
        {
            vm.Timetable[zastavka] = jr.Where(jr => jr.ZastavkaNazev == zastavka).ToList();
        }

        return View(vm);
    }
}