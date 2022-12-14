using System.Collections.Generic;
using System.Linq;
using NoKates.WebAdmin.Clients;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoKates.Common.Models;

namespace NoKates.WebAdmin.Pages
{
    public class RolesModel : PageModel
    {
        public List<Role> Roles { get; set; }
        public RolesModel()
        {
            Roles = new List<Role>();

        }
        public void OnGet()
        {
            var token = Request.Cookies["AccessToken"];
            if (string.IsNullOrWhiteSpace(token))
            {
                HttpContext.RedirectToRelativePath("Login");
                return;
            }

            var RoleClient = new RoleClient(token)
            {
                BaseUrl = GlobalConfig.RoleEndpointUrl,
            };
            Roles = RoleClient.GetAll().OrderBy(u => u.Name).ToList();

        }
    }
}
