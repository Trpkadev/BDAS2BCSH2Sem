using Microsoft.AspNetCore.Mvc.Filters;

namespace BCSH2BDAS2.Helpers;

public sealed class CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DecryptIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            KeyValuePair<string, object?> encryptedIdParam = context.ActionArguments.FirstOrDefault(arg => arg.Value is string && arg.Key.Equals("encryptedId", StringComparison.OrdinalIgnoreCase));

            if (encryptedIdParam.Value is string encryptedId)
            {
                int decryptedId = OurCryptography.Instance.DecryptId(encryptedId);
                context.HttpContext.Items["decryptedId"] = decryptedId;
            }
            base.OnActionExecuting(context);
        }
    }
}