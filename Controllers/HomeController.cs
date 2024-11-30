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
        return View();
    }

    [HttpGet]
    [Route("Plan")]
    public async Task<IActionResult> Plan()
    {
        try
        {
            if (ActingUser == null)
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Index));
            }
            var zastavky = await _context.GetZastavkyAsync() ?? [];
            return View(zastavky);
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction(nameof(Index), "Home");
        }
    }

    [HttpPost]
    [Route("Plan")]
    public async Task<ActionResult> Plan(string? od, string? _do, string? cas)
    {
        try
        {
            if (ActingUser == null)
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction(nameof(Plan));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Plan));
            }

            // TODO Použít funkci v DB a zobrazit výsledek
            var zastavky = await _context.GetZastavkyAsync();
            ViewBag.Zastavky = new SelectList(zastavky);
            return View();
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction(nameof(Index), "Home");
        }
    }

    [HttpGet]
    [Route("Timetable")]
    public async Task<ActionResult> Timetable(string? encryptedId)
    {
        try
        {
            if (ActingUser == null)
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction(nameof(Plan));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Plan));
            }

            var vm = new TimetableViewModel()
            {
                Routes = await _context.GetLinkyAsync() ?? [],
            };
            if (encryptedId == null)
                return View(vm);

            int linkaId = OurCryptography.Instance.DecryptId(encryptedId);
            var linka = await _context.GetLinkaByIdAsync(linkaId);
            if (linka == null)
            {
                SetErrorMessage("Objekt v databázi neexistuje");
                return RedirectToAction(nameof(Index));
            }

            var jr = await _context.JizdniRady.FromSql($"SELECT JR.*, Z.NAZEV AS NAZEV_ZASTAVKY FROM JIZDNI_RADY JR JOIN SPOJE S ON S.ID_SPOJ = JR.ID_SPOJ JOIN ZASTAVKY Z ON JR.ID_ZASTAVKA = Z.ID_ZASTAVKA WHERE ID_LINKA = {linkaId}").ToListAsync();
            var zastavky = jr.Select(jr => jr.NazevZastavky).Distinct().ToList();

            vm.Timetable = [];
            vm.CisloLinky = linka.Cislo;

            foreach (var zastavka in zastavky.Cast<string>())
                vm.Timetable[zastavka] = jr.Where(jr => jr.NazevZastavky == zastavka).ToList();

            return View(vm);
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}