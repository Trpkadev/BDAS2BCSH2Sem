using BCSH2BDAS2.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BCSH2BDAS2.Controllers;

[GetLoggedInUser]
[Route("Statistics")]
public class StatisticsController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("DBObjects")]
    public async Task<IActionResult> DBObjects()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction("Index", "Home");
            }

            var objekty = await _context.GetDBObjektyAsync();
            return View(objekty);
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpGet]
    [Route("VehiclesExpenses")]
    public async Task<IActionResult> VehiclesExpenses()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasMaintainerRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToAction("Index", "Home");
            }

            var naklady = await _context.GetNakladyNaVozidla();
            return View(naklady);
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToAction("Index", "Home");
        }
    }
}