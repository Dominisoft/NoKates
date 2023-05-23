using NoKates.Common.Infrastructure.Helpers;

namespace NoKates.Admin.Helpers
{
    public static class RedirectHelper
    {
        private const string UnknownService = "Unknown Service";
        public static void RedirectToRelativePath(this HttpContext context, string relativePath)
        {
            var scheme = context.Request.Scheme;
            var host = context.Request.Host;
            var appName = AppHelper.GetAppName();
            var path = $"{scheme}://{host}/{appName}/{relativePath}";
            if (appName == UnknownService)
                path = $"{scheme}://{host}/{relativePath}";
            context.Response.Redirect(path);
        }
    }
}
