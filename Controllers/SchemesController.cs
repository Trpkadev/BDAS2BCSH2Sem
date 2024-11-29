using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Schemes")]
public class SchemesController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("CreateEdit")]
    public async Task<IActionResult> CreateEdit(string? encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            if (encryptedId != null)
            {
                int id = GetDecryptedId(encryptedId);
                var schema = await _context.GetSchemataByIdAsync(id);
                if (schema == null)
                    return StatusCode(404);
                return View(schema);
            }
            return View(new Schema());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("CreateEditSubmit")]
    public async Task<IActionResult> CreateEditSubmit([FromForm] Schema schema)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return View(schema);

            if (schema.UploadedFile != null)
            {
                using var memoryStream = new MemoryStream();
                await schema.UploadedFile.CopyToAsync(memoryStream);
                schema.Soubor = memoryStream.ToArray();
                schema.NazevSouboru = schema.UploadedFile.FileName;
                schema.TypSouboru = schema.UploadedFile.ContentType;
                schema.VelikostSouboru = (int)memoryStream.Length;
                await _context.DMLSchemataAsync(schema);
            }
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
    public async Task<IActionResult> DeleteSubmit([FromForm] Schema schema)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);
            if (await _context.GetSchemataByIdAsync(schema.IdSchema) != null)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM SCHEMATA WHERE ID_SCHEMA = {0}", schema.IdSchema);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("Detail")]
    public async Task<IActionResult> Detail(string encryptedId)
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
    [Route("Download")]
    public async Task<IActionResult> Download(string encryptedId)
    {
        try
        {
            if (!ModelState.IsValid)
                return StatusCode(400);

            int id = GetDecryptedId(encryptedId);
            var schema = await _context.GetSchemataByIdAsync(id);
            if (schema == null)
                return StatusCode(404);
            return File(schema.Soubor!, schema.TypSouboru!);
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
            List<Schema>? schemata = await _context.GetSchemataAsync() ?? [];
            return View(schemata);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}