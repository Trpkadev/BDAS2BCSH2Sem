using BCSH2BDAS2.Core;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;

namespace BCSH2BDAS2.Controllers;

[Route("Users")]
public class UsersController(TransportationContext context) : Controller
{
    private readonly TransportationContext _context = context;

    // Login
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string username, string password)
    {
        if (Authentification.Login(username, password))
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return View();
        }
    }

    // Logout
    public IActionResult Logout()
    {
        Authentification.Logout();
        return RedirectToAction("Index", "Home");
    }

    // Register
    public IActionResult Register()
    {
        return View();
    }

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public IActionResult Register([Bind("IdUzivatel,Username,Password,Email,Role")] Uzivatel uzivatel)
    //{
    //	if (ModelState.IsValid)
    //	{
    //		_context.Database.ExecuteSqlRawAsync("INSERT INTO ST69612.UZIVATELE (ID_UZIVATEL, USERNAME, PASSWORD, EMAIL, ROLE) VALUES ({0}, {1}, {2}, {3}, {4})", uzivatel.IdUzivatel, uzivatel.Username, uzivatel.Password, uzivatel.Email, uzivatel.Role);
    //		return RedirectToAction(nameof(Login));
    //	}
    //	return View(uzivatel);
    //}
}