using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> Details(int id)
    {
        if (!ModelState.IsValid)
            return StatusCode(400);

        var vozidlo = await _context.GetVozidloById(id);
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
        await _context.Database.ExecuteSqlRawAsync("INSERT INTO VOZIDLA (ROK_VYROBY, NAJETE_KILOMETRY, KAPACITA, MA_KLIMATIZACI, ID_GARAZ, ID_MODEL) VALUES ({0}, {1}, {2}, {3}, {4}, {5})", vozidlo.RokVyroby, vozidlo.NajeteKilometry, vozidlo.Kapacita, vozidlo.MaKlimatizaci, vozidlo.IdGaraz, vozidlo.IdModel);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Route("Edit")]
    public async Task<IActionResult> Edit(int id)
    {
        if (!ModelState.IsValid)
            return StatusCode(400);

        var vozidlo = await _context.GetVozidloById(id);
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
            await _context.Database.ExecuteSqlRawAsync("UPDATE VOZIDLA SET ROK_VYROBY = {0}, NAJETE_KILOMETRY = {1}, KAPACITA = {2}, MA_KLIMATIZACI = {3}, ID_GARAZ = {4}, ID_MODEL = {5} WHERE ID_VOZIDLO = {6}", vozidlo.RokVyroby, vozidlo.NajeteKilometry, vozidlo.Kapacita, vozidlo.MaKlimatizaci, vozidlo.IdGaraz, vozidlo.IdModel, vozidlo.IdVozidlo);
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
    public async Task<IActionResult> Delete(int id)
    {
        if (!ModelState.IsValid)
            return StatusCode(400);

        var vozidlo = await _context.GetVozidloById(id);
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