using BCSH2BDAS2.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("ImportExport")]
public class ImportExportController(TransportationContext context, IHttpContextAccessor accessor)
    : BaseController(context, accessor)
{
    [HttpGet]
    [Route("ExportCSV")]
    public async Task<IActionResult> ExportCsv(string? nazev, char oddelovac = ';')
    {
        ViewBag.Nazev = nazev;
        ViewBag.Oddelovac = oddelovac;

        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToHome();
            }

            var objekty = await _context.GetDBObjektyAsync();
            var tabulky = objekty.Where(o => o.Typ is "TABLE" or "VIEW");
            ViewBag.Tabulky = new SelectList(tabulky);

            if (nazev != null)
            {
                var csv = await _context.GetTabulkaDoCsv(nazev, oddelovac);
                ViewBag.Csv = csv;
            }

            return View();
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    [HttpGet]
    [Route("ExportCSVDownload")]
    public async Task<IActionResult> ExportCsvDownload(string nazev, char oddelovac = ';')
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToHome();
            }

            var csv = await _context.GetTabulkaDoCsv(nazev, oddelovac);
            using var ms = new MemoryStream();
            await using var tw = new StreamWriter(ms);
            await tw.WriteAsync(csv);
            return File(ms.ToArray(), "text/csv", $"Export_{nazev}.csv");
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    [HttpGet]
    [Route("ImportRecords")]
    public IActionResult ImportRecords()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToHome();
            }

            return View();
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    [HttpPost]
    [Route("ImportRecordsSubmit")]
    public async Task<IActionResult> ImportRecordsSubmit(IFormFile soubor, char oddelovac)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToHome();
            }

            await using var stream = soubor.OpenReadStream();
            using var sr = new StreamReader(stream);
            var csv = await sr.ReadToEndAsync();
            csv = csv.Replace("\r\n", "\n");

            await _context.ImportRecords(csv, oddelovac);

            return RedirectToAction(nameof(ImportRecords));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }
}