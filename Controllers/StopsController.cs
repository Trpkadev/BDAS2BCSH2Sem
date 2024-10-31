using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using static BCSH2BDAS2.Helpers.CustomAttributes;

namespace BCSH2BDAS2.Controllers;

[Route("Stops")]
public class StopsController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Zastavky.FromSqlRaw("SELECT * FROM ZASTAVKY").ToListAsync());
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

        var zastavka = await _context.GetZastavkaById((int)id);
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
    [DecryptId]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Needed for decryption")]
    public async Task<IActionResult> Edit(string encryptedId)
    {
        int? id = (int?)HttpContext.Items["decryptedId"];
        if (id == null || !ModelState.IsValid)
            return StatusCode(400);

        var zastavka = await _context.GetZastavkaById((int)id);
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
    [DecryptId]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Needed for decryption")]
    public async Task<IActionResult> Delete(string encryptedId)
    {
        int? id = (int?)HttpContext.Items["decryptedId"];
        if (id == null || !ModelState.IsValid)
            return StatusCode(400);

        var zastavka = await _context.GetZastavkaById((int)id);
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