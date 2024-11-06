using BCSH2BDAS2.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BCSH2BDAS2.Helpers;

[AttributeUsage(AttributeTargets.Class)]
public class GetLoggedInUserAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is BaseController baseController)
        {
            baseController.ViewData["LoggedUser"] = baseController.LoggedUser;
            baseController.ViewData["ActingUser"] = baseController.ActingUser;
        }
        base.OnActionExecuting(context);
    }
}