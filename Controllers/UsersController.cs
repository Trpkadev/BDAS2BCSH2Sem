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
    [Route("AdminPanel")]
    public IActionResult AdminPanel()
    {
        try
        {
            //SELECT* FROM UZIVATELE JOIN ROLE ON(UZIVATELE.ID_ROLE = ROLE.ID_ROLE)
            return View(_context.Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE"));
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
            var uzivatel = await _context.GetUzivatelById(id);
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
            if (!ModelState.IsValid)
                return StatusCode(400);
            if (await _context.GetZastavkaById(uzivatel.IdUzivatel) != null)
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
    public IActionResult Login([FromForm] Uzivatel uzivatel)
    {
        try
        {
            if (!ModelState.IsValid)
                return StatusCode(400);
            if (LoginInternal(uzivatel.Jmeno, uzivatel.Heslo))
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("Login");
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("Logout")]
    public IActionResult Logout()
    {
        try
        {
            LogoutInternal();
            return RedirectToAction("Index", "Home");
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
            var hash = OurCryptography.EncryptHash(uzivatel.Heslo);
            await _context.Database.ExecuteSqlRawAsync("INSERT INTO UZIVATELE (JMENO, HESLO, ID_ROLE) VALUES ({0}, {1}, 1)", uzivatel.Jmeno, hash);
            return RedirectToAction(nameof(Login));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}