using BCSH2BDAS2.Helpers;
using BCSH2BDAS2.Models;
using Microsoft.AspNetCore.Mvc;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Roles")]
public class RolesController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
                return RedirectToAction(nameof(Index), "Home");
            return View(await _context.GetRoleAsync());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}