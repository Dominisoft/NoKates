using Microsoft.AspNetCore.Mvc.RazorPages;
using NoKates.Common.Infrastructure.Configuration;

namespace NoKates.WebAdmin.Pages
{
    public class EnvironmentViewerModel : PageModel
    {
        
        public void OnGet()
        {

        }

        public string DependencyEndpointUrl => ConfigurationValues.Values["DependencyEndpointUrl"];
    }
}
