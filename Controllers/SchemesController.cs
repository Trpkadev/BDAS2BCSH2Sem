using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Schemes")]
public class SchemesController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("Create")]
    public IActionResult Create()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");

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
    public async Task<IActionResult> CreateSubmit([FromForm] Schema schema)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return View(schema);
            await _context.DMLSchemataAsync(schema);
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
            var schema = await _context.GetSchemataByIdAsync(id);
            if (schema == null)
                return StatusCode(404);

            return View(schema);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("DeleteSubmit")]
    public async Task<IActionResult> DeleteSubmit([FromForm] Zastavka schema)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);
            if (await _context.GetSchemataByIdAsync(schema.IdZastavka) != null)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM ZASTAVKY WHERE ID_ZASTAVKA = {0}", schema.IdZastavka);

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
            var schema = await _context.GetSchemataByIdAsync(id);
            if (schema == null)
                return StatusCode(404);

            return View(schema);
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
            var schema = await _context.GetSchemataByIdAsync(id);
            if (schema == null)
                return StatusCode(404);

            var pasma = await _context.TarifniPasma.FromSqlRaw("SELECT * FROM TARIFNI_PASMA").ToListAsync();
            var pasmaList = new SelectList(pasma, "IdPasmo", "Nazev");
            ViewBag.Pasma = pasmaList;

            return View(schema);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("EditSubmit")]
    public async Task<IActionResult> EditSubmit([FromForm] Zastavka schema)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return View(schema);
            await _context.Database.ExecuteSqlRawAsync("UPDATE ZASTAVKY SET NAZEV = {0}, SOURADNICE_X = {1}, SOURADNICE_Y = {2}, ID_PASMO = {3} WHERE ID_ZASTAVKA = {4}", schema.Nazev, schema.SouradniceX, schema.SouradniceY, schema.IdPasmo, schema.IdZastavka);
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
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            List<Schema>? schemata = await _context.GetSchemataAsync();
            return View(schemata);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}