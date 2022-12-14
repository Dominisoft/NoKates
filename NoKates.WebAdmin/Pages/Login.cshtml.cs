using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoKates.Common.Infrastructure.Extensions;

namespace NoKates.WebAdmin.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task OnPost()
        {
            var form = Request.Form;
            var userEmail = form["Input.Email"];
            var userPassword = "FakePassword";//form["Input.Password"];
            var token = await GetToken(userEmail, userPassword);
            StoreToken(token);

            var path = Request.Query.ContainsKey("RedirectUrl") ? Request.Query["RedirectUrl"].ToString().TrimStart('/') : string.Empty;

            HttpContext.RedirectToRelativePath(path);
        }

      

        private async Task<string> GetToken(string user, string pass)
        {

            var values = new Dictionary<string, string>
  {
      { "Username", user },
      { "EncryptedPassword", pass}
  };

            var content = new StringContent(values.Serialize(), Encoding.UTF8, "application/json") ;

            var client = new HttpClient();

            var response = await client.PostAsync(GlobalConfig.AuthenticationUrl, content);

            var responseString = await response.Content.ReadAsStringAsync();
            return responseString.Trim('"');
        }
    }
}
