using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
                return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Index));
            }

            int id = GetDecryptedId(encryptedId);
            ActBehalfInternal(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(string encryptedId)
    {
        try
        {
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Index));
            }

            int id = GetDecryptedId(encryptedId);
            var uzivatel = await _context.GetUzivatelByIdAsync(id);
            if (uzivatel != null)
                return View(uzivatel);
            SetErrorMessage("Objekt v databázi neexistuje");
            return View(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
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
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Index));
            }

            if (await _context.GetUzivatelByIdAsync(pracovnik.IdUzivatel) == null)
                SetErrorMessage("Objekt v databázi neexistuje");
            else
            {
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM UZIVATELE WHERE ID_UZIVATEL = {0}", pracovnik.IdUzivatel);
                SetSuccessMessage();
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("Detail")]
    public async Task<IActionResult> Detail(string encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Index));
            }

            int id = GetDecryptedId(encryptedId);
            var uzivatel = await _context.GetUzivatelByIdAsync(id);
            if (uzivatel != null)
                return View(uzivatel);
            SetErrorMessage("Objekt v databázi neexistuje");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction("Index", "Home");
            }

            var uzivatele = await _context.GetUzivateleAsync();
            ViewData["Role"] = await _context.GetRoleAsync();
            return View(uzivatele);
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction("Index", "Home");
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
            SetErrorMessage("Chyba serveru");
            return RedirectToAction("Index", "Home");
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
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction("Index", "Home");
            }

            if (await LoginInternal(uzivatel.UzivatelskeJmeno, uzivatel.Heslo))
                return RedirectToAction("Index", "Home");
            SetErrorMessage("Nesprávné jméno nebo heslo");
            return RedirectToAction("Index", "Home");
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction("Index", "Home");
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
            SetErrorMessage("Chyba serveru");
            return RedirectToAction("Index", "Home");
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
                SetErrorMessage("Jméno již existuje");
                return View(uzivatel);
            }

            uzivatel.Heslo = OurCryptography.EncryptHash(uzivatel.Heslo);
            uzivatel.IdRole = 1;
            await _context.DMLUzivateleAsync(uzivatel);
            SetSuccessMessage("Registrace proběhla úspěšně!");
            return RedirectToAction(nameof(Login));
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction("Index", "Home");
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
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction("Index", "Home");
            }

            ActBehalfInternal(null);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction("Index", "Home");
        }
    }
}