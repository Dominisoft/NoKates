using NoKates.Common.Infrastructure.Client;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Identity.Common.DataTransfer;

namespace NoKates.Identity.Common.Clients
{
    public interface IRoleClient :IBaseClient<RoleDto>
    {

    }
    public class RoleClient : BaseClient<RoleDto>, IRoleClient
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
