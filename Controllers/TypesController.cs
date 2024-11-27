using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Types")]
public class TypesController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
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
                var typVozidla = await _context.GetTypy_VozidelByIdAsync(id);
                if (typVozidla != null)
                    return StatusCode(404);
                return View(typVozidla);
            }
            return View(new TypVozidla());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("CreateEditSubmit")]
    public async Task<IActionResult> CreateEditSubmit([FromForm] TypVozidla typVozidla)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return View(typVozidla);

            await _context.DMLTypy_VozidelAsync(typVozidla);
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
            var typVozidla = await _context.GetTypy_VozidelByIdAsync(id);
            if (typVozidla == null)
                return StatusCode(404);
            return View(typVozidla);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("DeleteSubmit")]
    public async Task<IActionResult> DeleteSubmit([FromForm] TypVozidla typVozidla)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            if (await _context.GetTypy_VozidelByIdAsync(typVozidla.IdTypVozidla) != null)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM TYPY_VOZIDEL WHERE ID_TYP_VOZIDLA = {0}", typVozidla.IdTypVozidla);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");

            var typyVozidel = await _context.GetTypy_VozidelAsync() ?? [];
            return View(typyVozidel);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}