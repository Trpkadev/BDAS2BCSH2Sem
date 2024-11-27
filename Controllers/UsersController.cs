﻿using BCSH2BDAS2.Helpers;
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
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            var uzivatele = await _context.GetUzivateleAsync();
            return View(uzivatele);
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
            var uzivatel = await _context.GetUzivateleByIdAsync(id);
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
    public async Task<IActionResult> DeleteSubmit([FromForm] Uzivatel pracovnik)
    {
        try
        {
            if (LoggedUser == null || !LoggedUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);
            if (await _context.GetUzivateleByIdAsync(pracovnik.IdUzivatel) != null)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM UZIVATELE WHERE ID_UZIVATEL = {0}", pracovnik.IdUzivatel);

            return RedirectToAction(nameof(Index));
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
            if (await LoginInternal(uzivatel.UzivatelskeJmeno, uzivatel.Heslo))
                return RedirectToAction(nameof(Index), "Home");
            TempData["error"] = "Nesprávné jméno nebo heslo";
            return RedirectToAction(nameof(Login));
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
            if ((await _context.GetUzivatelOrPracovnikUsernameExistsAsync(uzivatel.UzivatelskeJmeno)))
            {
                TempData["error"] = "Jméno již existuje";
                return View(uzivatel);
            }
            uzivatel.Heslo = OurCryptography.EncryptHash(uzivatel.Heslo);
            await _context.DMLUzivateleAsync(uzivatel);
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
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}