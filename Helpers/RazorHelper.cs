using Microsoft.AspNetCore.Mvc.Rendering;

namespace BCSH2BDAS2.Helpers;

public static class RazorHelper
{
    public static bool IsControllerAndAction(ViewContext viewContext, string controller, string? action = null)
    {
        return viewContext.RouteData.Values["controller"]?.Equals(controller) == true &&
               (action == null || viewContext.RouteData.Values["action"]?.Equals(action) == true);
    }

    public static bool IsControllerAndActionList(ViewContext viewContext, List<Tuple<string, string?>> controllerActionTupleList)
    {
        return controllerActionTupleList.Exists(controllerActionTuple =>
            viewContext.RouteData.Values["controller"]?.Equals(controllerActionTuple.Item1) == true &&
            (controllerActionTuple.Item2 == null || viewContext.RouteData.Values["action"]?.Equals(controllerActionTuple.Item2) == true));
    }
}