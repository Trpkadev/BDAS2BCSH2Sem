using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

public class StopsController(TransportationContext context) : Controller
{
    private readonly TransportationContext _context = context;

    // GET: Stops
    public async Task<IActionResult> Index()
    {
        return View(await _context.Zastavky.FromSqlRaw("SELECT * FROM ST69612.ZASTAVKY").ToListAsync());
    }

    // GET: Stops/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return StatusCode(404);

        var zastavka = await _context.Zastavky
            .FromSqlRaw("SELECT * FROM ST69612.ZASTAVKY WHERE ID_ZASTAVKA = {0}", id).FirstOrDefaultAsync();
        if (zastavka == null)
            return StatusCode(404);

        return View(zastavka);
    }

    // GET: Stops/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Stops/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdZastavka,Nazev,SouradniceX,SouradniceY,IdPasmo")] Zastavka zastavka)
    {
        if (ModelState.IsValid)
        {
            await _context.Database.ExecuteSqlRawAsync("INSERT INTO ST69612.ZASTAVKY (ID_ZASTAVKA, NAZEV, SOURADNICE_X, SOURADNICE_Y, ID_PASMO) VALUES ({0}, {1}, {2}, {3}, {4})", zastavka.IdZastavka, zastavka.Nazev, zastavka.SouradniceX, zastavka.SouradniceY, zastavka.IdPasmo);
            return RedirectToAction(nameof(Index));
        }
        return View(zastavka);
    }

    // GET: Stops/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return StatusCode(404);

        var zastavka = await _context.Zastavky.FromSqlRaw("SELECT * FROM ST69612.ZASTAVKY WHERE ID_ZASTAVKA = {0}", id).FirstOrDefaultAsync();
        if (zastavka == null)
            return StatusCode(404);

        return View(zastavka);
    }

    // POST: Stops/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdZastavka,Nazev,SouradniceX,SouradniceY,IdPasmo")] Zastavka zastavka)
    {
        if (id != zastavka.IdZastavka)
            return StatusCode(404);

        if (ModelState.IsValid)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("UPDATE ST69612.ZASTAVKY SET NAZEV = {0}, SOURADNICE_X = {1}, SOURADNICE_Y = {2}, ID_PASMO = {3} WHERE ID_ZASTAVKA = {4}", zastavka.Nazev, zastavka.SouradniceX, zastavka.SouradniceY, zastavka.IdPasmo, zastavka.IdZastavka);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZastavkaExists(zastavka.IdZastavka))
                    return StatusCode(404);
                return StatusCode(500);
            }
        }
        return View(zastavka);
    }

    // GET: Stops/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return StatusCode(404);

        var zastavka = await _context.Zastavky
            .FromSqlRaw("SELECT * FROM ST69612.ZASTAVKY WHERE ID_ZASTAVKA = {0}", id).FirstOrDefaultAsync();
        if (zastavka == null)
            return StatusCode(404);

        return View(zastavka);
    }

    // POST: Stops/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var zastavka = await _context.Zastavky.FindAsync(id);
        if (zastavka != null)
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM ST69612.ZASTAVKY WHERE ID_ZASTAVKA = {0}", id);

        return RedirectToAction(nameof(Index));
    }

    private bool ZastavkaExists(int id)
    {
        return _context.Zastavky.FromSqlRaw("SELECT * FROM ST69612.ZASTAVKY WHERE ID_ZASTAVKA = {0}", id).Any();
    }
}