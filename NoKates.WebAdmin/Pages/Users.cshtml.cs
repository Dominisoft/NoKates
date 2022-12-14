using System.Collections.Generic;
using System.Linq;
using NoKates.WebAdmin.Clients;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoKates.Common.Models;

namespace NoKates.WebAdmin.Pages
{
    public class UsersModel : PageModel
    {
        public List<User> Users { get; set; }
        public UsersModel()
        {
            Users = new List<User>();

        }
        public void OnGet()
        {
            var token = Request.Cookies["AccessToken"];
            if (string.IsNullOrWhiteSpace(token))
            {
                HttpContext.RedirectToRelativePath("Login");
                return;
            }

            var userClient = new UserClient(token)
            {
                BaseUrl = GlobalConfig.UserEndpointUrl,
            };
            Users = userClient.GetAll().OrderBy(u => u.Username).ToList();

        }
    }
}
