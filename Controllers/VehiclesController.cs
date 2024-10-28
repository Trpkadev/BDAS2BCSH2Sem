using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

public class VehiclesController(TransportationContext context) : Controller
{
    private readonly TransportationContext _context = context;

    // GET: Vehicles
    public async Task<IActionResult> Index()
    {
        return View(await _context.Vozidla.FromSqlRaw("SELECT * FROM ST69612.VOZIDLA").ToListAsync());
    }

    // GET: Vehicles/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (!ModelState.IsValid)
            return StatusCode(400);
        if (id == null)
            return StatusCode(404);

        var vozidlo = await _context.Vozidla
            .FromSqlRaw("SELECT * FROM ST69612.VOZIDLA WHERE ID_VOZIDLO = {0}", id).FirstOrDefaultAsync();

        if (vozidlo == null)
            return StatusCode(404);

        return View(vozidlo);
    }

    // GET: Vehicles/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Vehicles/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdVozidlo,RokVyroby,NajeteKilometry,Kapacita,MaKlimatizaci,IdGaraz,IdModel")] Vozidlo vozidlo)
    {
        if (ModelState.IsValid)
        {
            await _context.Database.ExecuteSqlRawAsync("INSERT INTO ST69612.VOZIDLA (ID_VOZIDLO, ROK_VYROBY, NAJETE_KILOMETRY, KAPACITA, MA_KLIMATIZACI, ID_GARAZ, ID_MODEL) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})", vozidlo.IdVozidlo, vozidlo.RokVyroby, vozidlo.NajeteKilometry, vozidlo.Kapacita, vozidlo.MaKlimatizaci, vozidlo.IdGaraz, vozidlo.IdModel);
            return RedirectToAction(nameof(Index));
        }
        return View(vozidlo);
    }

    // GET: Vehicles/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (!ModelState.IsValid)
            return StatusCode(400);
        if (id == null)
            return StatusCode(404);

        var vozidlo = await _context.Vozidla.FromSqlRaw("SELECT * FROM ST69612.VOZIDLA WHERE ID_VOZIDLO = {0}", id).FirstOrDefaultAsync();
        if (vozidlo == null)
            return StatusCode(404);

        return View(vozidlo);
    }

    // POST: Vehicles/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdVozidlo,RokVyroby,NajeteKilometry,Kapacita,MaKlimatizaci,IdGaraz,IdModel")] Vozidlo vozidlo)
    {
        if (id != vozidlo.IdVozidlo)
            return StatusCode(404);

        if (ModelState.IsValid)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("UPDATE ST69612.VOZIDLA SET ROK_VYROBY = {0}, NAJETE_KILOMETRY = {1}, KAPACITA = {2}, MA_KLIMATIZACI = {3}, ID_GARAZ = {4}, ID_MODEL = {5} WHERE ID_VOZIDLO = {6}", vozidlo.RokVyroby, vozidlo.NajeteKilometry, vozidlo.Kapacita, vozidlo.MaKlimatizaci, vozidlo.IdGaraz, vozidlo.IdModel, vozidlo.IdVozidlo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VozidloExists(vozidlo.IdVozidlo))
                    return StatusCode(404);
                return StatusCode(500);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(vozidlo);
    }

    // GET: Vehicles/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (!ModelState.IsValid)
            return StatusCode(400);
        if (id == null)
            return StatusCode(404);

        var vozidlo = await _context.Vozidla
            .FromSqlRaw("SELECT * FROM ST69612.VOZIDLA WHERE ID_VOZIDLO = {0}", id).FirstOrDefaultAsync();
        if (vozidlo == null)
            return StatusCode(404);

        return View(vozidlo);
    }

    // POST: Vehicles/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!ModelState.IsValid)
            return StatusCode(400);
        var vozidlo = await _context.Vozidla.FromSqlRaw("SELECT * FROM ST69612.VOZIDLA WHERE ID_VOZIDLO = {0}", id).FirstOrDefaultAsync();
        if (vozidlo != null)
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM ST69612.VOZIDLA WHERE ID_VOZIDLO = {0}", id);

        return RedirectToAction(nameof(Index));
    }

    private bool VozidloExists(int id)
    {
        return _context.Vozidla.FromSqlRaw("SELECT * FROM ST69612.VOZIDLA WHERE ID_VOZIDLO = {0}", id).Any();
    }
}