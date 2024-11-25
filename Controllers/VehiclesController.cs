﻿using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Vehicles")]
public class VehiclesController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("Create")]
    public IActionResult Create()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
                return RedirectToAction(nameof(Index), "Home");

            var garaze = _context.Garaze.FromSqlRaw("SELECT * FROM GARAZE").ToList();
            var garazeList = new SelectList(garaze, "IdGaraz", "Nazev");
            ViewBag.Garaze = garazeList;
            var modely = _context.Modely.FromSqlRaw("SELECT * FROM MODELY").ToList();
            var modelyList = new SelectList(modely, "IdModel", "Nazev");
            ViewBag.Modely = modelyList;

            return View();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("CreateSubmit")]
    public async Task<IActionResult> CreateSubmit([FromForm] Vozidlo vozidlo)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return View(vozidlo);
            await _context.DMLVozidlaAsync(vozidlo);
            return RedirectToAction(nameof(Index));
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
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);
            int id = GetDecryptedId(encryptedId);
            var vehicle = await _context.GetVozidlaByIdAsync(id);
            if (vehicle == null)
                return StatusCode(404);

            return View(vehicle);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> DeleteSubmit([FromForm] Vozidlo vozidlo)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM VOZIDLA WHERE ID_VOZIDLO = {0}", vozidlo.IdVozidlo);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("Details")]
    public async Task<IActionResult> Details(string encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);
            int id = GetDecryptedId(encryptedId);
            var vozidlo = await _context.GetVozidlaByIdAsync(id);
            if (vozidlo == null)
                return StatusCode(404);

            return View(vozidlo);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("Edit")]
    public async Task<IActionResult> Edit(string encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);

            int id = GetDecryptedId(encryptedId);
            var vozidlo = await _context.GetVozidlaByIdAsync(id);
            if (vozidlo == null)
                return StatusCode(404);

            var garaze = await _context.Garaze.FromSqlRaw("SELECT * FROM GARAZE").ToListAsync();
            var garazeList = new SelectList(garaze, "IdGaraz", "Nazev");
            ViewBag.Garaze = garazeList;
            var modely = await _context.Modely.FromSqlRaw("SELECT * FROM MODELY").ToListAsync();
            var modelyList = new SelectList(modely, "IdModel", "Nazev");
            ViewBag.Modely = modelyList;

            return View(vozidlo);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("EditSubmit")]
    public async Task<IActionResult> EditSubmit([FromForm] Vozidlo vozidlo)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
                return RedirectToAction(nameof(Index), "Home");
            if (!ModelState.IsValid)
                return StatusCode(400);
            await _context.DMLVozidlaAsync(vozidlo);
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _context.GetVozidlaByIdAsync(vozidlo.IdVozidlo) == null)
                return StatusCode(404);
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
                return RedirectToAction(nameof(Index), "Home");
            return View(await _context.GetVozidlaAsync());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}