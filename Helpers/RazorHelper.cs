using Microsoft.AspNetCore.Mvc.Rendering;

namespace BCSH2BDAS2.Helpers;

public static class RazorHelper
{
    public static bool IsControllerAndActionList(ViewContext viewContext, List<Tuple<string, string?>> controllerActionTupleList)
    {
        foreach (Tuple<string, string?> controllerActionTuple in controllerActionTupleList)
            if (viewContext.RouteData.Values["controller"]?.Equals(controllerActionTuple.Item1) == true && (controllerActionTuple.Item2 == null || viewContext.RouteData.Values["action"]?.Equals(controllerActionTuple.Item2) == true))
                return true;
        return false;
    }

    public static bool IsControllerAndAction(ViewContext viewContext, string controller, string? action = null)
    {
        return viewContext.RouteData.Values["controller"]?.Equals(controller) == true && (action == null || viewContext.RouteData.Values["action"]?.Equals(action) == true);
    }
}