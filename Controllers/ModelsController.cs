using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Models")]
public class ModelsController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
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

            ViewData["Znacky"] = await _context.GetZnackyAsync() ?? [];
            ViewData["TypyVozidel"] = await _context.GetTypy_VozidelAsync() ?? [];
            if (encryptedId != null)
            {
                int id = GetDecryptedId(encryptedId);
                var model = await _context.GetModelyByIdAsync(id);
                if (model == null)
                    return StatusCode(404);
            }
            return View(new Model());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("CreateEditSubmit")]
    public async Task<IActionResult> CreateSubmit([FromForm] Model model)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return View("CreateEdit", model);

            if (await _context.GetModelyByIdAsync(model.IdModel) != null)
                await _context.DMLModelyAsync(model);
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
            var model = await _context.GetModelyByIdAsync(id);
            if (model == null)
                return StatusCode(404);
            return View(model);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("DeleteSubmit")]
    public async Task<IActionResult> DeleteSubmit([FromForm] Model model)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            if (await _context.GetModelyByIdAsync(model.IdModel) != null)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM MODELY WHERE ID_MODEL = {0}", model.IdModel);
            return RedirectToAction(nameof(Index));
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

            var modely = await _context.GetModelyAsync() ?? [];
            return View(modely);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}