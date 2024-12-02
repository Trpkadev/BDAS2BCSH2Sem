using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Users")]
public class UsersController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("ActBehalf")]
    public IActionResult ActBehalf(string encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
                return RedirectToHome();
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Index));
            }

            int id = GetDecryptedId(encryptedId);
            ActBehalfInternal(id);
            return RedirectToHome();
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
            if (ActingUser == null || !ActingUser.HasAdminRights())
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
            var uzivatel = await _context.GetUzivatelByIdAsync(id);
            if (uzivatel != null)
                return View(uzivatel);
            SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            return View(nameof(Index));
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
    public async Task<IActionResult> DeleteSubmit([FromForm] Uzivatel pracovnik)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToAction(nameof(Index));
            }

            if (await _context.GetUzivatelByIdAsync(pracovnik.IdUzivatel) == null)
                SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            else
            {
                await _context.DeleteFromTableAsync("UZIVATELE", "ID_UZIVATEL", pracovnik.IdUzivatel);
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
            if (ActingUser == null || !ActingUser.HasAdminRights())
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
            var uzivatel = await _context.GetUzivatelByIdAsync(id);
            if (uzivatel != null)
                return View(uzivatel);
            SetErrorMessage(Resource.DB_DATA_NOT_EXIST);
            return RedirectToAction(nameof(Index));
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
            if (ActingUser == null || !ActingUser.HasAdminRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToHome();
            }

            var uzivatele = await _context.GetUzivateleAsync();
            ViewData["Role"] = await _context.GetRoleAsync();
            return View(uzivatele);
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    [HttpGet]
    [Route("Login")]
    public IActionResult Login()
    {
        try
        {
            return View();
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("LoginSubmit")]
    public async Task<IActionResult> Login([FromForm] Uzivatel uzivatel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToHome();
            }

            if (await LoginInternal(uzivatel.UzivatelskeJmeno, uzivatel.Heslo))
                return RedirectToHome();
            SetErrorMessage(Resource.INVALID_LOGIN);
            return RedirectToHome();
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    [HttpGet]
    [Route("Logout")]
    public IActionResult Logout()
    {
        try
        {
            if (LoggedUser != null)
                LogoutInternal();
            return RedirectToHome();
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    [HttpGet]
    [Route("Register")]
    public IActionResult Register()
    {
        try
        {
            return View();
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("RegisterSubmit")]
    public async Task<IActionResult> Register([FromForm] Uzivatel uzivatel)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(uzivatel);
            if (await _context.GetUzivatelUsernameExistsAsync(uzivatel.UzivatelskeJmeno))
            {
                SetErrorMessage(Resource.REGISTER_NAME_EXISTS);
                return View(uzivatel);
            }

            uzivatel.Heslo = OurCryptography.EncryptHash(uzivatel.Heslo);
            uzivatel.IdRole = 1;
            await _context.DMLUzivateleAsync(uzivatel);
            SetSuccessMessage(Resource.REGISTER_SUCCESS);
            return RedirectToAction(nameof(Login));
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }

    [HttpGet]
    [Route("StopActingBehalf")]
    public IActionResult StopActingBehalf()
    {
        try
        {
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToHome();
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage(Resource.INVALID_REQUEST_DATA);
                return RedirectToHome();
            }

            ActBehalfInternal(null);
            return RedirectToHome();
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }
}