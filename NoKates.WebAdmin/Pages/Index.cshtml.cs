using Microsoft.AspNetCore.Mvc.RazorPages;
using NoKates.Common.Infrastructure.Configuration;

namespace NoKates.WebAdmin.Pages
{
    public class IndexModel : PageModel
    {
        public string DependencyEndpointUrl => ConfigurationValues.Values["OverviewEndpointUrl"];

        public void OnGet()
        {
            var token = Request.Cookies["AccessToken"];
            if (string.IsNullOrWhiteSpace(token))
            {
                HttpContext.RedirectToRelativePath("Login");
                return;
            }

        }

    }
}
