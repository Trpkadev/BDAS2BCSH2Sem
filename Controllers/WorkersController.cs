using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Workers")]
public class WorkersController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpPost]
    [Route("AddPay")]
    public async Task<IActionResult> AddPay(double multiplier, int minPay)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasManagerRights())
                return RedirectToHome();

            await _context.NavyseniPlatuAsync(multiplier, minPay);
            SetSuccessMessage();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("CreateEdit")]
    public async Task<IActionResult> CreateEdit(string? encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasWorkerRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Index));
            }

            var uzivatele = await _context.GetUzivateleAsync();
            var pracovnici = await _context.GetPracovniciAsync();
            if (encryptedId == null)
            {
                ViewBag.Uzivatele = new SelectList(uzivatele, "IdUzivatel", "");
                ViewBag.Pracovnici = new SelectList(pracovnici, "IdPracovnik", "");
                return View(new Pracovnik());
            }

            int id = GetDecryptedId(encryptedId);
            var pracovnik = await _context.GetPracovnikByIdAsync(id) ?? await _context.GetPracovnikByUserIdAsync(id);

            if (pracovnik != null)
            {
                ViewBag.Uzivatele = new SelectList(uzivatele, "IdUzivatel", "", pracovnik.IdUzivatel);
                ViewBag.Pracovnici = new SelectList(pracovnici, "IdPracovnik", "", pracovnik.IdNadrizeny);
                return View(pracovnik);
            }
            SetErrorMessage(Resource.DB_DATA_NOT_EXIST + ", nebyl vytvořen účet pracovníka - kontaktujte manažera nebo administrátora");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [Route("CreateEditSubmit")]
    public async Task<IActionResult> CreateEditSubmit([FromForm] Pracovnik pracovnik)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasWorkerRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToHome();
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return View(nameof(CreateEdit), pracovnik);
            }

            if (pracovnik.IdPracovnik != 0 && await _context.GetPracovnikByIdAsync(pracovnik.IdPracovnik) == null)
                SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            else
            {
                await _context.DMLPracovniciAsync(pracovnik);
                SetSuccessMessage();
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(string encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasManagerRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToHome();
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Index));
            }

            int id = GetDecryptedId(encryptedId);
            var pracovnik = await _context.GetPracovnikByIdAsync(id);
            if (pracovnik != null)
                return View(pracovnik);
            SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction(nameof(Index));
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("DeleteSubmit")]
    public async Task<IActionResult> DeleteSubmit([FromForm] int idPracovnik)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasManagerRights())
                return RedirectToHome();
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Index));
            }

            if (await _context.GetPracovnikByIdAsync(idPracovnik) == null)
                SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            else
            {
                await _context.DeleteFromTableAsync("PRACOVNICI", [("ID_PRACOVNIK", idPracovnik.ToString())]);
                SetSuccessMessage();
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("Detail")]
    public async Task<IActionResult> Detail(string encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasWorkerRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Index));
            }

            int id = GetDecryptedId(encryptedId);
            var pracovnik = await _context.GetPracovnikByIdAsync(id);
            if (pracovnik != null)
                return View(pracovnik);
            SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            return View(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasManagerRights())
                return RedirectToHome();

            var pracovnici = await _context.GetPracovniciAsync() ?? [];
            return View(pracovnici);
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    [HttpGet]
    [Route("Hierarchy")]
    public async Task<IActionResult> Hierarchy()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasManagerRights())
                return RedirectToHome();

            var pracovnici = await _context.GetPracovniciHierarchieAsync();

            return View(pracovnici);
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }
}