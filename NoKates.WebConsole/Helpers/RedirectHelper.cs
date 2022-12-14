using NoKates.Common.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;

namespace NoKates.WebConsole.Helpers
{
    public static class RedirectHelper
    {
        private const string UNKNOWN_SERVICE = "Unknown Service";
        public static void RedirectToRelativePath(this HttpContext context, string relativePath)
        {
            var scheme = context.Request.Scheme;
            var host = context.Request.Host;
            var appName = AppHelper.GetAppName();
            var path = $"{scheme}://{host}/{appName}/{relativePath}";
            if (appName == UNKNOWN_SERVICE)
                path = $"{scheme}://{host}/{relativePath}";
            context.Response.Redirect(path);
        }
    }
}
