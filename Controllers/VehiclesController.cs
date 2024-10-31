using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Vehicles")]
public class VehiclesController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("Create")]
    public IActionResult Create()
    {
        try
        {
            return View();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("CreateSubmit")]
    public async Task<IActionResult> CreateSubmit([FromForm] Vozidlo vozidlo)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(vozidlo);
            await _context.CreateVozidlo(vozidlo);
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
            if (!ModelState.IsValid)
                return StatusCode(400);
            int id = GetDecryptedId(encryptedId);
            var vozidlo = await _context.GetVozidloById(id);
            if (vozidlo == null)
                return StatusCode(404);

            return View(vozidlo);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> DeleteSubmit([FromForm] Vozidlo vozidlo)
    {
        try
        {
            if (!ModelState.IsValid)
                return StatusCode(400);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM VOZIDLA WHERE ID_VOZIDLO = {0}", vozidlo.IdVozidlo);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("Details")]
    public async Task<IActionResult> Details(string encryptedId)
    {
        try
        {
            if (!ModelState.IsValid)
                return StatusCode(400);
            int id = GetDecryptedId(encryptedId);
            var vozidlo = await _context.GetVozidloById(id);
            if (vozidlo == null)
                return StatusCode(404);

            return View(vozidlo);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("Edit")]
    public async Task<IActionResult> Edit(string encryptedId)
    {
        try
        {
            if (!ModelState.IsValid)
                return StatusCode(400);
            int id = GetDecryptedId(encryptedId);
            var vozidlo = await _context.GetVozidloById(id);
            if (vozidlo == null)
                return StatusCode(404);

            return View(vozidlo);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("EditSubmit")]
    public async Task<IActionResult> EditSubmit([FromForm] Vozidlo vozidlo)
    {
        try
        {
            if (!ModelState.IsValid)
                return StatusCode(400);
            await _context.Database.ExecuteSqlRawAsync("UPDATE VOZIDLA SET ROK_VYROBY = {0}, NAJETE_KILOMETRY = {1}, KAPACITA = {2}, MA_KLIMATIZACI = {3}, ID_GARAZ = {4}, ID_MODEL = {5} WHERE ID_VOZIDLO = {6}", vozidlo.RokVyroby, vozidlo.NajeteKilometry, vozidlo.Kapacita, vozidlo.MaKlimatizaci ? 1 : 0, vozidlo.IdGaraz, vozidlo.IdModel, vozidlo.IdVozidlo);
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _context.GetVozidloById(vozidlo.IdVozidlo) == null)
                return StatusCode(404);
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            return View(await _context.Vozidla.FromSqlRaw("SELECT * FROM VOZIDLA").ToListAsync());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}