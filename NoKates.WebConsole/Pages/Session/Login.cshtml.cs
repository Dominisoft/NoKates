using NoKates.Identity.Common.Clients;
using NoKates.WebConsole.Extensions;
using NoKates.WebConsole.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NoKates.WebConsole.Pages.Session
{
    public class LoginModel : PageModel
    {
        private readonly IAuthenticationClient _authenticationClient;

        public LoginModel(IAuthenticationClient authenticationClient)
        {
            _authenticationClient = authenticationClient;
        }

        public void OnPost()
        {
            var form = Request.Form;
            var userEmail = form["Input.Email"];
            var userPassword = "FakePassword";//form["Input.Password"];
            var response = _authenticationClient.Authenticate(userEmail, userPassword);
            if (response.IsError)
            {
                HttpContext.RedirectToRelativePath($"./Login?Message={response.Message.Substring(0,50)}");
            }

            var token = response.Object;
            if (TokenHelper.TokenIsInvalid(token))
            {
                Response.Redirect("./Login?Message=\"Invalid Response from Server\"");
                return;
            }
            Response.SetToken(token);
            
            var path = Request.Query.ContainsKey("RedirectUrl") ? Request.Query["RedirectUrl"].ToString().TrimStart('/') : string.Empty;

            HttpContext.RedirectToRelativePath(path);
        }

   

     
    }
}
