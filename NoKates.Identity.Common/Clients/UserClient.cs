using NoKates.Common.Infrastructure.Client;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.Identity.Common.Clients
{
    public interface IUserClient : IBaseClient<User>
    {

    }
    public class UserClient : BaseClient<User>, IUserClient
    {
        public UserClient(string baseUrl, string token) : this(baseUrl)
        {
            HttpHelper.SetToken(token);
        }

        public UserClient(string baseUrl)
        {
            base.BaseUrl = baseUrl+"/User";
        }
    }
}
