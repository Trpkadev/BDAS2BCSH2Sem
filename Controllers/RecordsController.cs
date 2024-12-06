using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Records")]
public class RecordsController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("CreateEdit")]
    public async Task<IActionResult> CreateEdit(string? encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.JizdniRady = new SelectList(await _context.GetJizdniRadyAsync());
            ViewBag.Vozidla = new SelectList(await _context.GetVozidlaAsync());

            if (encryptedId == null)
                return View(new ZaznamTrasy());

            int id = GetDecryptedId(encryptedId);
            var zaznamTrasy = await _context.GetZaznam_TrasyByIdAsync(id);
            if (zaznamTrasy != null)
                return View(zaznamTrasy);

            SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            return View(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction(nameof(Index));
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("CreateEditSubmit")]
    public async Task<IActionResult> CreateEditSubmit([FromForm] ZaznamTrasy zaznamTrasy)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Index), "Home");
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(CreateEdit), zaznamTrasy);
            }

            if (zaznamTrasy.IdZaznam != 0 && await _context.GetZaznam_TrasyByIdAsync(zaznamTrasy.IdZaznam) == null)
                SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            else
            {
                await _context.DMLZaznamy_TrasyAsync(zaznamTrasy);
                SetSuccessMessage();
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(string encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Index));
            }

            int id = GetDecryptedId(encryptedId);
            var zaznamTrasy = await _context.GetZaznam_TrasyByIdAsync(id);
            if (zaznamTrasy != null)
                return View(zaznamTrasy);
            SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction(nameof(Index));
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("DeleteSubmit")]
    public async Task<IActionResult> DeleteSubmit([FromForm] ZaznamTrasy zaznamTrasy)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Index));
            }

            if (await _context.GetZaznam_TrasyByIdAsync(zaznamTrasy.IdZaznam) == null)
                SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            else
            {
                await _context.DeleteFromTableAsync("ZAZNAMY_TRASY", [("ID_LINKA", zaznamTrasy.IdZaznam.ToString())]);
                SetSuccessMessage();
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("Detail")]
    public async Task<IActionResult> Detail(string encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Index));
            }

            int id = GetDecryptedId(encryptedId);
            var zaznamTrasy = await _context.GetZaznam_TrasyByIdAsync(id);
            if (zaznamTrasy != null)
                return View(zaznamTrasy);
            SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToHome();
            }

            var zaznamyTras = await _context.GetZaznamy_TrasyAsync() ?? [];
            return View(zaznamyTras);
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    [HttpGet]
    [Route("AverageDelay")]
    public async Task<IActionResult> AverageDelay(int? cislo, int? pocetDni, int? hodina)
    {
        // todo nefunguje
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToHome();
            }

            var linky = await _context.GetLinkyAsync();
            ViewBag.Linky = new SelectList(linky);

            if (cislo != null && pocetDni != null)
            {
                var idLinka = linky.Where(l => l.Cislo == (int)cislo).Select(l => l.IdLinka).First();
                ViewBag.Vysledek = await _context.GetPrumerneZpozdeni(idLinka, (int)pocetDni, hodina);

                ViewBag.CisloLinky = cislo;
                ViewBag.PocetDni = pocetDni;
                ViewBag.Hodina = hodina;
            }

            return View();
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }
}