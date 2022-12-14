using NoKates.Common.Infrastructure.Client;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.Identity.Common.Clients
{
    public interface IRoleClient :IBaseClient<Role>
    {

    }
    public class RoleClient : BaseClient<Role>, IRoleClient
    {
        public RoleClient(string baseUrl, string token) : this(baseUrl)
        {
            HttpHelper.SetToken(token);
        }

        public RoleClient(string baseUrl)
        {
            base.BaseUrl = baseUrl+"/Role";
        }
    }
}
