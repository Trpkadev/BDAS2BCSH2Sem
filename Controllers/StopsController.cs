using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");

            var pasma = _context.TarifniPasma.FromSqlRaw("SELECT * FROM TARIFNI_PASMA").ToList();
            var pasmaList = new SelectList(pasma, "IdPasmo", "Nazev");
            ViewBag.Pasma = pasmaList;

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
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return View(zastavka);
            await _context.DMLZastavkyAsync(zastavka);
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
            var zastavka = await _context.GetZastavkaByIdAsync(id);
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
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);
            if (await _context.GetZastavkaByIdAsync(zastavka.IdZastavka) != null)
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
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);
            int id = GetDecryptedId(encryptedId);
            var zastavka = await _context.GetZastavkaByIdAsync(id);
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
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            int id = GetDecryptedId(encryptedId);
            var zastavka = await _context.GetZastavkaByIdAsync(id);
            if (zastavka == null)
                return StatusCode(404);

            var pasma = await _context.TarifniPasma.FromSqlRaw("SELECT * FROM TARIFNI_PASMA").ToListAsync();
            var pasmaList = new SelectList(pasma, "IdPasmo", "Nazev");
            ViewBag.Pasma = pasmaList;

            return View(zastavka);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("EditSubmit")]
    public async Task<IActionResult> EditSubmit([FromForm] Zastavka zastavka)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return View(zastavka);
            await _context.Database.ExecuteSqlRawAsync("UPDATE ZASTAVKY SET NAZEV = {0}, SOURADNICE_X = {1}, SOURADNICE_Y = {2}, ID_PASMO = {3} WHERE ID_ZASTAVKA = {4}", zastavka.Nazev, zastavka.SouradniceX, zastavka.SouradniceY, zastavka.IdPasmo, zastavka.IdZastavka);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            if (await _context.GetZastavkaByIdAsync(zastavka.IdZastavka) == null)
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
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            List<Zastavka>? zastavky = await _context.GetZastavkyAsync();
            //TODO Join table Tarifni_Pasma
            return View(zastavky);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}