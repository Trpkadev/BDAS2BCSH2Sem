using BCSH2BDAS2.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BCSH2BDAS2.Controllers;

[Route("Logs")]
[GetLoggedInUser]
public class LogsController(TransportationContext context, IHttpContextAccessor accessor) : BaseController(context, accessor)
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            if (ActingUser == null || !ActingUser.HasAdminRights())
            {
                SetErrorMessage(Resource.INVALID_PERMISSIONS);
                return RedirectToHome();
            }

            var logy = await _context.GetLogyAsync() ?? [];
            return View(logy);
        }
        catch (Exception)
        {
            SetErrorMessage(Resource.GENERIC_SERVER_ERROR);
            return RedirectToHome();
        }
    }
}