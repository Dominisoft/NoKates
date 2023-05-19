using Microsoft.AspNetCore.Mvc;
using NoKates.Admin.Helpers;

namespace NoKates.Admin.Extensions
{
    public static class RequestExtensions
    {
        public static string GetToken(this HttpRequest request)
            => request.Cookies["AuthorizationToken"];

        public static void SetToken(this HttpResponse response, string token)
        {
            var option = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1)
            };

            if (TokenHelper.TokenIsInvalid(token))
            {
                response.Redirect("./Login?Message=\"Invalid Response from Server\"");
                return;
            }

            response.Cookies.Append("AuthorizationToken", token, option);
        }

        public static ActionResult ToActionResult(this bool isSuccess)
        {
            if (isSuccess)
                return new OkResult();
            throw new Exception("Failed");
        }
    }
}
