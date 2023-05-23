using System.IdentityModel.Tokens.Jwt;
using Blazored.LocalStorage;

namespace NoKates.Admin.Helpers
{
    public static class TokenHelper
    {
        public static bool TokenIsInvalid(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return true;

            if (token.Split('.').Length != 3)
                return true;

            var t = new JwtSecurityToken(token);

            if (t.ValidFrom > DateTime.Now)
                return true;

            if (t.ValidTo < DateTime.Now)
                return true;


            return false;
        }

        public static async Task<string> GetToken(this ILocalStorageService storage)
        {
            return await storage.GetItemAsStringAsync("AuthorizationToken");
        }
    }
}
