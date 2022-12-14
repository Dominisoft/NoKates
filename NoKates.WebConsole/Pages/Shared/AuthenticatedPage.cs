using System.Collections.Generic;
using NoKates.Common.Models;
using NoKates.WebConsole.Extensions;
using NoKates.WebConsole.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NoKates.WebConsole.Pages.Shared
{
    public interface IAuthenticatedPage
    {

    }
    public abstract class AuthenticatedPage : PageModel, IAuthenticatedPage
    {
        public void OnGet()
        {
            var token = Request.GetToken();
            if (TokenHelper.TokenIsInvalid(token))
            {
                HttpContext.RedirectToRelativePath("Session/Login");
                return;
            }

            LoadData(token);
        }
        public abstract void LoadData(string token);   
    }
}
