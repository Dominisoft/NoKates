using System.Collections.Generic;
using NoKates.Common.Infrastructure.Client;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.WebAdmin.Clients
{
    public class RoleClient:BaseClient<Role>
    {
        public RoleClient(string token)
        {
            Token = token;
            HttpHelper.SetToken(token);
        }

        public string Token { get; }
        public RestResponse<List<Role>> GetAll()
        {
            return HttpHelper.Get<List<Role>>($"{BaseUrl}/All", $"{Token}");
        }
    }
}
