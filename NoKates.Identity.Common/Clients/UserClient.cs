using NoKates.Common.Infrastructure.Client;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Identity.Common.DataTransfer;

namespace NoKates.Identity.Common.Clients
{
    public interface IUserClient : IBaseClient<UserDto>
    {

    }
    public class UserClient : BaseClient<UserDto>, IUserClient
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
