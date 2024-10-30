using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[Route("Users")]
public class UsersController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
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
		if (LoginInternal(uzivatel.Jmeno, uzivatel.Heslo))
			return RedirectToAction("Index", "Home");
		else
            return RedirectToAction("Login");
    }

    [HttpGet]
    [Route("Logout")]
    public IActionResult Logout()
    {
        LogoutInternal();
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
    public async Task<IActionResult> Register([FromForm] Uzivatel uzivatel)
    {
        if (!ModelState.IsValid)
            return View(uzivatel);
        var password = OurCryptography.Encrypt(uzivatel.Heslo);
        await _context.Database.ExecuteSqlRawAsync("INSERT INTO UZIVATELE (JMENO, HESLO, ID_ROLE) VALUES ({0}, {1}, 1)", uzivatel.Jmeno, password);
        return RedirectToAction(nameof(Login));
    }
}