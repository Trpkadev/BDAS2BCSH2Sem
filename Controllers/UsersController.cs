using BCSH2BDAS2.Core;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[Route("Users")]
public class UsersController(TransportationContext context) : Controller
{
    private readonly TransportationContext _context = context;

    [HttpGet]
    [Route("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("LoginSubmit")]
    public IActionResult Login([FromForm] Uzivatel uzivatel)
    {
        if (!ModelState.IsValid)
            return StatusCode(400);
        if (Authentification.Login(uzivatel.Jmeno, uzivatel.Heslo))
            return RedirectToAction("Index", "Home");
        else
            return View();
    }

    [HttpPost]
    [Route("LogoutSubmit")]
    public IActionResult Logout()
    {
        Authentification.Logout();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("Register")]
    public IActionResult Register()
    {
        return View();
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("RegisterSubmit")]
    public IActionResult Register([FromForm] Uzivatel uzivatel)
    {
        if (!ModelState.IsValid)
            return View(uzivatel);
        _context.Uzivatele.FromSqlRaw("INSERT INTO ST69612.UZIVATELE (USERNAME, PASSWORD, ID_ROLE) VALUES ({0}, {1}, {2})", uzivatel.Jmeno, uzivatel.Heslo, uzivatel.IdRole);
        return RedirectToAction(nameof(Login));
    }
}