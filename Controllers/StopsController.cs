using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[Route("Stops")]
public class StopsController(TransportationContext context) : Controller
{
    private readonly TransportationContext _context = context;

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Zastavky.FromSqlRaw("SELECT * FROM ZASTAVKY").ToListAsync());
    }

    [HttpGet]
    [Route("Details")]
    public async Task<IActionResult> Details(int id)
    {
        if (!ModelState.IsValid)
            return StatusCode(400);

        var zastavka = await _context.GetZastavkaById(id);
        if (zastavka == null)
            return StatusCode(404);

        return View(zastavka);
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
    public async Task<IActionResult> CreateSubmit([FromForm] Zastavka zastavka)
    {
        if (!ModelState.IsValid)
            return View(zastavka);
        await _context.Database.ExecuteSqlRawAsync("INSERT INTO ZASTAVKY (NAZEV, SOURADNICE_X, SOURADNICE_Y, ID_PASMO) VALUES ({0}, {1}, {2}, {3})", zastavka.Nazev, zastavka.SouradniceX, zastavka.SouradniceY, zastavka.IdPasmo);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Route("Edit")]
    public async Task<IActionResult> Edit(int id)
    {
        if (!ModelState.IsValid)
            return StatusCode(400);

        var zastavka = await _context.GetZastavkaById(id);
        if (zastavka == null)
            return StatusCode(404);

        return View(zastavka);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> EditSubmit([FromForm] Zastavka zastavka)
    {
        if (!ModelState.IsValid)
            return View(zastavka);
        try
        {
            await _context.Database.ExecuteSqlRawAsync("UPDATE ZASTAVKY SET NAZEV = {0}, SOURADNICE_X = {1}, SOURADNICE_Y = {2}, ID_PASMO = {3} WHERE ID_ZASTAVKA = {4}", zastavka.Nazev, zastavka.SouradniceX, zastavka.SouradniceY, zastavka.IdPasmo, zastavka.IdZastavka);
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _context.GetZastavkaById(zastavka.IdZastavka) == null)
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

        var zastavka = await _context.GetZastavkaById(id);
        if (zastavka == null)
            return StatusCode(404);

        return View(zastavka);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("DeleteSubmit")]
    public async Task<IActionResult> DeleteSubmit([FromForm] Zastavka zastavka)
    {
        if (!ModelState.IsValid)
            return StatusCode(400);
        if (await _context.GetZastavkaById(zastavka.IdZastavka) != null)
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM ZASTAVKY WHERE ID_ZASTAVKA = {0}", zastavka.IdZastavka);

        return RedirectToAction(nameof(Index));
    }
}