using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Vehicles")]
public class VehiclesController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
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

            ViewData["Garaze"] = await _context.GetGarazeAsync() ?? [];
            ViewData["Modely"] = await _context.GetModelyAsync() ?? [];
            if (encryptedId == null)
                return View(new Vozidlo());
            int id = GetDecryptedId(encryptedId);
            var vozidlo = await _context.GetVozidloByIdAsync(id);
            if (vozidlo != null)
                return View(vozidlo);
            SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("CreateEditSubmit")]
    public async Task<IActionResult> CreateEditSubmit([FromForm] Vozidlo vozidlo)
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
                return RedirectToAction(nameof(CreateEdit), vozidlo);
            }

            if (vozidlo.IdVozidlo != 0 && await _context.GetVozidloByIdAsync(vozidlo.IdVozidlo) == null)
                SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            else
            {
                await _context.DMLVozidlaAsync(vozidlo);
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
            var vehicle = await _context.GetVozidloByIdAsync(id);
            if (vehicle != null)
                return View(vehicle);
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
    [Route("")]
    public async Task<IActionResult> DeleteSubmit([FromForm] Vozidlo vozidlo)
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

            if (await _context.GetVozidloByIdAsync(vozidlo.IdVozidlo) == null)
                SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            else
            {
                await _context.DeleteFromTableAsync("VOZIDLA", "ID_VOZIDLO", vozidlo.IdVozidlo);
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
            var vozidlo = await _context.GetVozidloByIdAsync(id);
            if (vozidlo != null)
                return View(vozidlo);
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
                return RedirectToAction(nameof(Index), "Home");
            }

            var vozidla = await _context.GetVozidlaAsync() ?? [];
            return View(vozidla);
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }
}