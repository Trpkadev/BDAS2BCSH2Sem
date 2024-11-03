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
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);
            int id = GetDecryptedId(encryptedId);
            ActBehalfInternal(id);
            return RedirectToAction(nameof(AdminPanel));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("AdminPanel")]
    public async Task<IActionResult> AdminPanel()
    {
        try
        {
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            return View(await _context.GetUzivateleAsync());
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
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);
            int id = GetDecryptedId(encryptedId);
            var uzivatel = await _context.GetUzivatelByIdAsync(id);
            if (uzivatel == null)
                return StatusCode(404);

            return View(uzivatel);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("DeleteSubmit")]
    public async Task<IActionResult> DeleteSubmit([FromForm] Uzivatel uzivatel)
    {
        try
        {
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);
            if (await _context.GetZastavkaByIdAsync(uzivatel.IdUzivatel) != null)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM UZIVATELE WHERE ID_UZIVATEL = {0}", uzivatel.IdUzivatel);

            return RedirectToAction(nameof(AdminPanel));
        }
        catch (Exception)
        {
            return StatusCode(500);
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
            return StatusCode(500);
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
                return StatusCode(400);
            if (await LoginInternal(uzivatel.Jmeno, uzivatel.Heslo))
                return RedirectToAction(nameof(Index), "Home");
            else
                return RedirectToAction(nameof(Login));
        }
        catch (Exception) // při nesprávném jménu nebo heslu
        {
            return RedirectToAction(nameof(Login));
        }
    }

    [HttpGet]
    [Route("Logout")]
    public IActionResult Logout()
    {
        try
        {
            if (LoggedUser == null)
                return RedirectToAction(nameof(Index), "Home");
            LogoutInternal();
            return RedirectToAction(nameof(Index), "Home");
        }
        catch (Exception)
        {
            return StatusCode(500);
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
            return StatusCode(500);
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
            uzivatel.Heslo = OurCryptography.EncryptHash(uzivatel.Heslo);
            await _context.CreateUzivatelAsync(uzivatel);
            return RedirectToAction(nameof(Login));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("StopActingBehalf")]
    public IActionResult StopActingBehalf()
    {
        try
        {
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);
            ActBehalfInternal(null);
            return RedirectToAction(nameof(AdminPanel));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}