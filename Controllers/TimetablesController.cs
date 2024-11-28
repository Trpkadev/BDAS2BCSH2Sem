using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Timetables")]
public class TimetablesController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
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
                var jizdniRad = await _context.GetJizdni_RadyByIdAsync(id);
                if (jizdniRad == null)
                    return StatusCode(404);
                return View(jizdniRad);
            }
            return View(new JizdniRad());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("CreateEditSubmit")]
    public async Task<IActionResult> CreateEditSubmit([FromForm] JizdniRad jizdniRad)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return View(jizdniRad);

            await _context.DMLJizdni_RadyAsync(jizdniRad);
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
            var jizdniRad = await _context.GetJizdni_RadyByIdAsync(id);
            if (jizdniRad == null)
                return StatusCode(404);
            return View(jizdniRad);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("DeleteSubmit")]
    public async Task<IActionResult> DeleteSubmit([FromForm] JizdniRad jizdniRad)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            if (await _context.GetJizdni_RadyByIdAsync(jizdniRad.IdJizdniRad) != null)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM JIZDNI_RADY WHERE ID_JIZDNI_RAD = {0}", jizdniRad.IdJizdniRad);
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
            var jizdniRad = await _context.GetJizdni_RadyByIdAsync(id);
            if (jizdniRad == null)
                return StatusCode(404);
            return View(jizdniRad);
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

            var jizdniRady = await _context.GetJizdni_RadyAsync() ?? [];
            return View(jizdniRady);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}