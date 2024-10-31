using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Stops")]
public class StopsController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
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
    public async Task<IActionResult> CreateSubmit([FromForm] Zastavka zastavka)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(zastavka);
            await _context.CreateZastavka(zastavka);
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
            var zastavka = await _context.GetZastavkaById(id);
            if (zastavka == null)
                return StatusCode(404);

            return View(zastavka);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("DeleteSubmit")]
    public async Task<IActionResult> DeleteSubmit([FromForm] Zastavka zastavka)
    {
        try
        {
            if (!ModelState.IsValid)
                return StatusCode(400);
            if (await _context.GetZastavkaById(zastavka.IdZastavka) != null)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM ZASTAVKY WHERE ID_ZASTAVKA = {0}", zastavka.IdZastavka);

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
            var zastavka = await _context.GetZastavkaById(id);
            if (zastavka == null)
                return StatusCode(404);

            return View(zastavka);
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
            var zastavka = await _context.GetZastavkaById(id);
            if (zastavka == null)
                return StatusCode(404);

            return View(zastavka);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> EditSubmit([FromForm] Zastavka zastavka)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(zastavka);
            await _context.Database.ExecuteSqlRawAsync("UPDATE ZASTAVKY SET NAZEV = {0}, SOURADNICE_X = {1}, SOURADNICE_Y = {2}, ID_PASMO = {3} WHERE ID_ZASTAVKA = {4}", zastavka.Nazev, zastavka.SouradniceX, zastavka.SouradniceY, zastavka.IdPasmo, zastavka.IdZastavka);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            if (await _context.GetZastavkaById(zastavka.IdZastavka) == null)
                return StatusCode(404);
        }

        return StatusCode(500);
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            return View(await _context.Zastavky.FromSqlRaw("SELECT * FROM ZASTAVKY").ToListAsync());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}