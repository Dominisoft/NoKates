using System.Collections.Generic;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;

namespace NoKates.Common.Infrastructure.Client
{
    public interface IWebHostManagementClient
    {
        bool StartAppPool(string appPoolName, string token);
        bool StopAppPool(string appPoolName, string token);
        bool AddAppPool(string appPoolName, string token);
        bool RemoveAppPool(string appPoolName, string token);
        bool AddApp(string appPoolName, string token);
        bool RemoveApp(string appPoolName, string token);
        Dictionary<string, bool> GetAppPools(string token);
        List<string> GetUnreferencedDirectories(string token);

    }
    public class WebHostManagementClient : IWebHostManagementClient
    {
        private readonly string _baseUrl;
        public WebHostManagementClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public bool StartAppPool(string appPoolName, string token)
            => !HttpHelper.Post($"{_baseUrl}/PoolManagement/Start/{appPoolName}",token).IsError;

        public bool StopAppPool(string appPoolName, string token)
            => !HttpHelper.Post($"{_baseUrl}/PoolManagement/Stop/{appPoolName}", token).IsError;

        public bool AddAppPool(string appPoolName, string token)
            => !HttpHelper.Post($"{_baseUrl}/PoolManagement/AddPool/{appPoolName}", token).IsError;

        public bool RemoveAppPool(string appPoolName, string token)
            => !HttpHelper.Post($"{_baseUrl}/PoolManagement/RemovePool/{appPoolName}", token).IsError;

        public bool AddApp(string appName, string token)
            => !HttpHelper.Post($"{_baseUrl}/PoolManagement/AddApp/{appName}", token).IsError;

        public bool RemoveApp(string appName, string token)
            => !HttpHelper.Post($"{_baseUrl}/PoolManagement/RemoveApp/{appName}", token).IsError;

        public Dictionary<string, bool> GetAppPools(string token)
            => HttpHelper.Get<Dictionary<string,bool>>($"{_baseUrl}/PoolManagement/AppPools", token).ThrowIfError();
        public List<string> GetUnreferencedDirectories(string token)
            => HttpHelper.Get<List<string>>($"{_baseUrl}/PoolManagement/UnreferencedDirectories", token).ThrowIfError();

    }
}
