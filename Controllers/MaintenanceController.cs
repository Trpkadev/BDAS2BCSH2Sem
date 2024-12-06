using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Maintenance")]
public class MaintenanceController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("CreateEdit")]
    public async Task<IActionResult> CreateEdit(string? encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Vozidla = new SelectList(await _context.GetVozidlaAsync());
            ViewData["o"] = new Oprava();
            ViewData["c"] = new Cisteni();

            if (encryptedId == null)
                return View(new Udrzba());
            int id = GetDecryptedId(encryptedId);
            var udrzba = await _context.GetUdrzbaByIdAsync(id);
            if (udrzba != null)
            {
                switch (udrzba)
                {
                    case Cisteni cisteni:
                        ViewData["c"] = cisteni;
                        break;
                    case Oprava oprava:
                        ViewData["o"] = oprava;
                        break;
                }
                return View(udrzba);
            }
            SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("CreateEditSubmit")]
    public async Task<IActionResult> CreateEditSubmit([FromForm] Udrzba udrzba, [FromForm] string? PopisUkonu, [FromForm] double? Cena, [FromForm] bool? CistenoOzonem, [FromForm] bool? UmytoVMycce)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(CreateEdit), udrzba);
            }

            if (udrzba.IdUdrzba != 0 && await _context.GetUdrzbaByIdAsync(udrzba.IdUdrzba) == null)
            {
                SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
                return RedirectToAction(nameof(Index));
            }
            switch (udrzba.TypUdrzby)
            {
                case 'c':
                    if (UmytoVMycce == null || CistenoOzonem == null)
                    {
                        SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                        return RedirectToAction(nameof(CreateEdit), udrzba);
                    }
                    var cisteni = new Cisteni
                    {
                        IdUdrzba = udrzba.IdUdrzba,
                        IdVozidlo = udrzba.IdVozidlo,
                        Datum = udrzba.Datum,
                        KonecUdrzby = udrzba.KonecUdrzby,
                        NazevVozidla = udrzba.NazevVozidla,
                        UmytoVMycce = (bool)UmytoVMycce,
                        CistenoOzonem = (bool)CistenoOzonem,
                        TypUdrzby = udrzba.TypUdrzby
                    };
                    await _context.DMLUdrzbyAsync(cisteni);
                    break;
                case 'o':
                    if (PopisUkonu == null || Cena == null)
                    {
                        SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                        return RedirectToAction(nameof(CreateEdit), udrzba);
                    }
                    var oprava = new Oprava
                    {
                        IdUdrzba = udrzba.IdUdrzba,
                        IdVozidlo = udrzba.IdVozidlo,
                        Datum = udrzba.Datum,
                        KonecUdrzby = udrzba.KonecUdrzby,
                        NazevVozidla = udrzba.NazevVozidla,
                        PopisUkonu = PopisUkonu,
                        Cena = (double)Cena,
                        TypUdrzby = udrzba.TypUdrzby
                    };
                    await _context.DMLUdrzbyAsync(oprava);
                    break;
                default:
                    await _context.DMLUdrzbyAsync(udrzba);
                    break;
            }
            SetSuccessMessage();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
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
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
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
            var udrzba = await _context.GetUdrzbaByIdAsync(id);
            if (udrzba != null)
                return View(udrzba);
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
    public async Task<IActionResult> DeleteSubmit([FromForm] Udrzba udrzba)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Index));
            }

            if (await _context.GetUdrzbaByIdAsync(udrzba.IdUdrzba) == null)
                SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            else
            {
                await _context.DeleteFromTableAsync("UDRZBY", [("ID_UDRZBA", udrzba.IdUdrzba.ToString())]);
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
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
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
            var udrzba = await _context.GetUdrzbaByIdAsync(id);
            if (udrzba != null)
                return View(udrzba);

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
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToHome();
            }

            var udrzba = await _context.GetUdrzbyAsync();
            return View(udrzba);
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }
}