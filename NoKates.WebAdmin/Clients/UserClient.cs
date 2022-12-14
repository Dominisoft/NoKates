using System.Collections.Generic;
using NoKates.Common.Infrastructure.Client;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.WebAdmin.Clients
{
    public class UserClient:BaseClient<User>
    {
        public UserClient(string token)
        {
            Token = token;
            HttpHelper.SetToken(token);
        }

        public string Token { get; }
    
    }
}
