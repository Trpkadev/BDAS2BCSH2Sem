using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Workers")]
public class WorkersController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpPost]
    [Route("AddPay")]
    public async Task<IActionResult> AddPay()
    {
        try
        {
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");

            var pracovnici = await _context.GetPracovniciAsync() ?? [];
            return View(pracovnici);
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("CreateEdit")]
    public async Task<IActionResult> CreateEdit(string? encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Uzivatele = new SelectList(await _context.GetUzivateleAsync());
            ViewBag.Pracovnici = new SelectList(await _context.GetPracovniciAsync());
            if (encryptedId == null)
                return View(new Pracovnik());
            int id = GetDecryptedId(encryptedId);
            var pracovnik = await _context.GetPracovnikByIdAsync(id);
            if (pracovnik != null)
                return View(pracovnik);
            SetErrorMessage("Objekt v databázi neexistuje");
            return View(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [Route("CreateEditSubmit")]
    public async Task<IActionResult> CreateEditSubmit([FromBody] Pracovnik pracovnik)
    {
        try
        {
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction(nameof(Index), "Home");
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Index));
            }

            if (await _context.GetPracovnikByIdAsync(pracovnik.IdPracovnik) == null)
                SetErrorMessage("Objekt v databázi neexistuje");
            else
            {
                await _context.DMLPracovniciAsync(pracovnik);
                SetSuccessMessage();
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(string encryptedId)
    {
        try
        {
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction(nameof(Index), "Home");
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Index));
            }

            int id = GetDecryptedId(encryptedId);
            var pracovnik = await _context.GetPracovnikByIdAsync(id);
            if (pracovnik != null)
                return View(pracovnik);
            SetErrorMessage("Objekt v databázi neexistuje");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction(nameof(Index));
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("DeleteSubmit")]
    public async Task<IActionResult> DeleteSubmit([FromForm] Pracovnik pracovnik)
    {
        try
        {
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Index));
            }

            if (await _context.GetPracovnikByIdAsync(pracovnik.IdPracovnik) == null)
                SetErrorMessage("Objekt v databázi neexistuje");
            else
            {
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM PRACOVNICI WHERE ID_UZIVATEL = {0}", pracovnik.IdPracovnik);
                SetSuccessMessage();
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("Detail")]
    public async Task<IActionResult> Detail(string encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Index));
            }

            int id = GetDecryptedId(encryptedId);
            var pracovnik = await _context.GetPracovnikByIdAsync(id);
            if (pracovnik != null)
                return View(pracovnik);
            SetErrorMessage("Objekt v databázi neexistuje");
            return View(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            if (LoggedUser == null || LoggedUser.HasManagerRights())
                return RedirectToAction(nameof(Index), "Home");

            var pracovnici = await _context.GetPracovniciAsync() ?? [];
            return View(pracovnici);
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction("Index", "Home");
        }
    }
}