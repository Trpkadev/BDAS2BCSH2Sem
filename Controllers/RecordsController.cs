using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            if (encryptedId != null)
            {
                int id = GetDecryptedId(encryptedId);
                var zaznamTrasy = await _context.GetZaznam_TrasyByIdAsync(id);
                if (zaznamTrasy == null)
                    return StatusCode(404);
                return View(zaznamTrasy);
            }
            return View(new ZaznamTrasy());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("CreateEditSubmit")]
    public async Task<IActionResult> CreateEditSubmit([FromForm] ZaznamTrasy zaznamTrasy)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return View("CreateEdit", zaznamTrasy);

            await _context.DMLZaznamy_TrasyAsync(zaznamTrasy);
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
            var zaznamTrasy = await _context.GetZaznam_TrasyByIdAsync(id);
            if (zaznamTrasy == null)
                return StatusCode(404);
            return View(zaznamTrasy);
        }
        catch (Exception)
        {
            return StatusCode(500);
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
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            if (await _context.GetZaznam_TrasyByIdAsync(zaznamTrasy.IdZaznam) != null)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM ZAZNAMY_TRASY WHERE ID_LINKA = {0}", zaznamTrasy.IdZaznam);
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
            var zaznamTrasy = await _context.GetZaznam_TrasyByIdAsync(id);
            if (zaznamTrasy == null)
                return StatusCode(404);
            return View(zaznamTrasy);
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

            var zaznamyTras = await _context.GetZaznamy_TrasyAsync() ?? [];
            return View(zaznamyTras);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}