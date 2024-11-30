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
        var objekty = await _context.GetDBObjektyAsync();
        return View(objekty);
    }

    [HttpGet]
    [Route("VehiclesExpenses")]
    public async Task<IActionResult> VehiclesExpenses()
    {
        var naklady = await _context.GetNakladyNaVozidla();
        return View(naklady);
    }
}