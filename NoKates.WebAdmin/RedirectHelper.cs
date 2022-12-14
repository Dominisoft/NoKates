using Microsoft.AspNetCore.Http;
using NoKates.Common.Infrastructure.Helpers;

namespace NoKates.WebAdmin
{
    public static class RedirectHelper
    {
        public static void RedirectToRelativePath(this HttpContext context, string relativePath)
        {
           var scheme = context.Request.Scheme;
            var host = context.Request.Host;
            var appName = AppHelper.GetAppName();
            


            var path = $"{scheme}://{host}/{appName}/{relativePath}";
            if (appName == "Unknown Service")
                path = $"{scheme}://{host}/{relativePath}";

            if (relativePath == "Login")
                path += $"?RedirectUrl={context.Request.Path}";
            context.Response.Redirect(path);
        }
    }
}
