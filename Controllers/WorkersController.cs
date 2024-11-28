using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Workers")]
public class WorkersController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("CreateEdit")]
    public async Task<IActionResult> CreateEdit(string? encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            ViewData["Role"] = await _context.GetRoleAsync() ?? [];
            if (encryptedId != null)
            {
                int id = GetDecryptedId(encryptedId);
                var pracovnik = await _context.GetPracovniciByIdAsync(id);
                if (pracovnik == null)
                    return StatusCode(404);
                return View(pracovnik);
            }
            return View(new Pracovnik());
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
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            int id = GetDecryptedId(encryptedId);
            var pracovnik = await _context.GetPracovniciByIdAsync(id);
            if (pracovnik == null)
                return StatusCode(404);
            return View(pracovnik);
        }
        catch (Exception)
        {
            return StatusCode(500);
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
                return StatusCode(400);

            if (await _context.GetPracovniciByIdAsync(pracovnik.IdPracovnik) != null)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM PRACOVNICI WHERE ID_UZIVATEL = {0}", pracovnik.IdPracovnik);
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
            var pracovnik = await _context.GetPracovniciByIdAsync(id);
            if (pracovnik == null)
                return StatusCode(404);
            return View(pracovnik);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Route("EditSubmit")]
    public async Task<IActionResult> EditSubmit([FromBody] Pracovnik pracovnik)
    {
        try
        {
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            if (await _context.GetPracovniciByIdAsync(pracovnik.IdPracovnik) != null)
                await _context.DMLPracovniciAsync(pracovnik);
            return StatusCode(200);
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
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");

            var pracovnici = await _context.GetPracovniciAsync() ?? [];
            ViewData["Role"] = await _context.GetRoleAsync() ?? [];
            return View(pracovnici);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}