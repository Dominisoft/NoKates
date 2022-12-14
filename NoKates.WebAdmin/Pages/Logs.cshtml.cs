using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.WebAdmin.Pages
{
    public class LogsModel : PageModel
    {
        public List<LogEntry> LogEntries;
        public LogsModel()
        {
            LogEntries = new List<LogEntry>();
        }
        public void OnGet()
        {
            var token = Request.Cookies["AccessToken"];
            if (string.IsNullOrWhiteSpace(token))
            {
                HttpContext.RedirectToRelativePath("Login");
                return;
            }
            var serviceName = Request.Query["ServiceName"];
            var path = $"{Request.Scheme}://{Request.Host}";
            path = $"http://localserver.wardkraft.com/{serviceName}/NoKates/Log";
           // throw new RequestException(200, $"Get From {path} \r\n With Auth: {token}");

            LogEntries = HttpHelper.Get<List<LogEntry>>(path, token);
        }
    }
}
