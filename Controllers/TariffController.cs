using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Tariff")]
public class TariffController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("CreateEdit")]
    public async Task<IActionResult> CreateEdit(string? encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            if (encryptedId != null)
            {
                int id = GetDecryptedId(encryptedId);
                var tarifniPasmo = await _context.GetTarifni_PasmaByIdAsync(id);
                if (tarifniPasmo == null)
                    return StatusCode(404);
                return View(tarifniPasmo);
            }
            return View(new TarifniPasmo());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("CreateEditSubmit")]
    public async Task<IActionResult> CreateEditSubmit([FromForm] TarifniPasmo tarifniPasmo)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return View(tarifniPasmo);

            await _context.DMLTarifni_PasmaAsync(tarifniPasmo);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(string encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            int id = GetDecryptedId(encryptedId);
            var tarifniPasmo = await _context.GetTarifni_PasmaByIdAsync(id);
            if (tarifniPasmo == null)
                return StatusCode(404);
            return View(tarifniPasmo);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("DeleteSubmit")]
    public async Task<IActionResult> DeleteSubmit([FromForm] TarifniPasmo tarifniPasmo)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            if (await _context.GetTarifni_PasmaByIdAsync(tarifniPasmo.IdPasmo) != null)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM TARIFNI_PASMA WHERE ID_PASMO = {0}", tarifniPasmo.IdPasmo);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("Detail")]
    public async Task<IActionResult> Detail(string encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            int id = GetDecryptedId(encryptedId);
            var tarifniPasmo = await _context.GetTarifni_PasmaByIdAsync(id);
            if (tarifniPasmo == null)
                return StatusCode(404);
            return View(tarifniPasmo);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");

            var tarifniPasma = await _context.GetTarifni_PasmaAsync() ?? [];
            return View(tarifniPasma);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}