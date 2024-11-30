﻿using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Timetables")]
public class TimetablesController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("CreateEdit")]
    public async Task<IActionResult> CreateEdit(string? encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Index));
            }

            if (encryptedId == null)
                return View(new JizdniRad());
            int id = GetDecryptedId(encryptedId);
            var jizdniRad = await _context.GetJizdni_RadByIdAsync(id);
            if (jizdniRad != null)
                return View(jizdniRad);
            SetErrorMessage("Objekt v databázi neexistuje");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction(nameof(Index));
        }
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("CreateEditSubmit")]
    public async Task<IActionResult> CreateEditSubmit([FromForm] JizdniRad jizdniRad)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction(nameof(Index), "Home");
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(CreateEdit), jizdniRad);
            }

            if (await _context.GetJizdni_RadByIdAsync(jizdniRad.IdJizdniRad) == null)
                SetErrorMessage("Objekt v databázi neexistuje");
            else
            {
                await _context.DMLJizdni_RadyAsync(jizdniRad);
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
    [Route("Delete")]
    public async Task<IActionResult> Delete(string encryptedId)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
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
            var jizdniRad = await _context.GetJizdni_RadByIdAsync(id);
            if (jizdniRad != null)
                return View(jizdniRad);
            SetErrorMessage("Objekt v databázi neexistuje");
            return RedirectToAction(nameof(Index));
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
    public async Task<IActionResult> DeleteSubmit([FromForm] JizdniRad jizdniRad)
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasDispatchRights())
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Neplatná data požadavku");
                return RedirectToAction(nameof(Index));
            }

            if (await _context.GetJizdni_RadByIdAsync(jizdniRad.IdJizdniRad) == null)
                SetErrorMessage("Objekt v databázi neexistuje");
            else
            {
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM JIZDNI_RADY WHERE ID_JIZDNI_RAD = {0}", jizdniRad.IdJizdniRad);
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
            var jizdniRad = await _context.GetJizdni_RadByIdAsync(id);
            if (jizdniRad != null)
                return View(jizdniRad);
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
            if (ActingUser == null || !ActingUser.HasAdminRights())
            {
                SetErrorMessage("Nedostačující oprávnění");
                return RedirectToAction("Index", "Home");
            }

            var jizdniRady = await _context.GetJizdni_RadyAsync() ?? [];
            return View(jizdniRady);
        }
        catch (Exception)
        {
            SetErrorMessage("Chyba serveru");
            return RedirectToAction("Index", "Home");
        }
    }
}