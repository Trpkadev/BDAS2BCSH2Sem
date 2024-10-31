using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using static BCSH2BDAS2.Helpers.CustomAttributes;

namespace BCSH2BDAS2.Controllers;

[Route("Vehicles")]
public class VehiclesController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Vozidla.FromSqlRaw("SELECT * FROM VOZIDLA").ToListAsync());
    }

    [HttpGet]
    [Route("Details")]
    [DecryptId]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Needed for decryption")]
    public async Task<IActionResult> Details(string encryptedId)
    {
        int? id = (int?)HttpContext.Items["decryptedId"];
        if (id == null || !ModelState.IsValid)
            return StatusCode(400);

        var vozidlo = await _context.GetVozidloById((int)id);
        if (vozidlo == null)
            return StatusCode(404);

        return View(vozidlo);
    }

    [HttpGet]
    [Route("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("CreateSubmit")]
    public async Task<IActionResult> CreateSubmit([FromForm] Vozidlo vozidlo)
    {
        if (!ModelState.IsValid)
            return View(vozidlo);
        await _context.CreateVozidlo(vozidlo);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Route("Edit")]
    [DecryptId]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Needed for decryption")]
    public async Task<IActionResult> Edit(string encryptedId)
    {
        int? id = (int?)HttpContext.Items["decryptedId"];
        if (id == null || !ModelState.IsValid)
            return StatusCode(400);

        var vozidlo = await _context.GetVozidloById((int)id);
        if (vozidlo == null)
            return StatusCode(404);

        return View(vozidlo);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("EditSubmit")]
    public async Task<IActionResult> EditSubmit([FromForm] Vozidlo vozidlo)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(vozidlo);
            await _context.Database.ExecuteSqlRawAsync("UPDATE VOZIDLA SET ROK_VYROBY = {0}, NAJETE_KILOMETRY = {1}, KAPACITA = {2}, MA_KLIMATIZACI = {3}, ID_GARAZ = {4}, ID_MODEL = {5} WHERE ID_VOZIDLO = {6}", vozidlo.RokVyroby, vozidlo.NajeteKilometry, vozidlo.Kapacita, vozidlo.MaKlimatizaci ? 1 : 0, vozidlo.IdGaraz, vozidlo.IdModel, vozidlo.IdVozidlo);
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _context.GetVozidloById(vozidlo.IdVozidlo) == null)
                return StatusCode(404);
        }
        return StatusCode(500);
    }

    [HttpGet]
    [Route("Delete")]
    [DecryptId]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Needed for decryption")]
    public async Task<IActionResult> Delete(string encryptedId)
    {
        int? id = (int?)HttpContext.Items["decryptedId"];
        if (id == null || !ModelState.IsValid)
            return StatusCode(400);

        var vozidlo = await _context.GetVozidloById((int)id);
        if (vozidlo == null)
            return StatusCode(404);

        return View(vozidlo);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> DeleteSubmit([FromForm] Vozidlo vozidlo)
    {
        if (!ModelState.IsValid)
            return StatusCode(400);
        if (await _context.GetVozidloById(vozidlo.IdVozidlo) != null)
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM VOZIDLA WHERE ID_VOZIDLO = {0}", vozidlo.IdVozidlo);

        return RedirectToAction(nameof(Index));
    }
}