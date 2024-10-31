using BCSH2BDAS2.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BCSH2BDAS2.Helpers;

[AttributeUsage(AttributeTargets.Class)]
public class GetLoggedInUserAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is not BaseController)
            return;
        BaseController baseController = (BaseController)context.Controller;
        baseController.ViewData["LoggedUser"] = baseController.LoggedUser;
        base.OnActionExecuting(context);
    }
}