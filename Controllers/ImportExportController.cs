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
    [Route("Export")]
    public async Task<IActionResult> Export(string? typ, string? nazev, char oddelovac = ';')
    {
        ViewBag.Typ = typ;
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

            if (typ != null && nazev != null)
            {
                if (typ == "csv")
                {
                    var csv = await _context.GetTabulkaDoCsvAsync(nazev, oddelovac);
                    ViewBag.Preview = csv;
                }
                else
                {
                    var json = await _context.GetTabulkaDoJsonAsync(nazev);
                    ViewBag.Preview = json;
                }
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
    [Route("ExportDownload")]
    public async Task<IActionResult> ExportDownload(string typ, string nazev, char oddelovac = ';')
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToHome();
            }

            string? data;
            if (typ == "csv")
            {
                data = await _context.GetTabulkaDoCsvAsync(nazev, oddelovac);
            }
            else
            {
                data = await _context.GetTabulkaDoJsonAsync(nazev);
            }

            using var ms = new MemoryStream();
            await using var tw = new StreamWriter(ms);
            await tw.WriteAsync(data);
            var arr = ms.ToArray();

            if (typ == "csv")
            {
                return File(arr, "text/csv", $"Export_{nazev}.csv");
            }
            else
            {
                return File(arr, "application/json", $"Export_{nazev}.json");
            }
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

            await _context.CSVDoZaznamuTrasyAsync(csv, oddelovac);

            SetSuccessMessage("Úspěšně naimportováno");
            return RedirectToAction(nameof(ImportRecords));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }
}