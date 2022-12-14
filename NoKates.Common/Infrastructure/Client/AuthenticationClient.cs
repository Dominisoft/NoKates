using System.Collections.Generic;
using NoKates.Common.Infrastructure.Helpers;

namespace NoKates.Common.Infrastructure.Client
{
    public class AuthenticationClient
    {
        private readonly string _authenticationUrl;

        public AuthenticationClient(string authenticationUrl)
        {
            _authenticationUrl = authenticationUrl;
        }

        public string GetToken(string user, string pass)
        {

            var values = new Dictionary<string, string>
  {
      { "Username", user },
      { "EncryptedPassword", pass}
  };
            var response = HttpHelper.Post(_authenticationUrl, values, string.Empty);

            return response?.Message?.Split('.').Length != 3 ? null : response.Message.Trim('"');
        }
    }
}
