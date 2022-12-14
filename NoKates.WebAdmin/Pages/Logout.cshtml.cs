using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NoKates.WebAdmin.Pages
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            Response.Cookies.Delete("AccessToken");
            HttpContext.RedirectToRelativePath("Login");

        }
    }
}
