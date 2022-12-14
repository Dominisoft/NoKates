using System.Collections.Generic;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.WebConsole.Application;

namespace NoKates.WebConsole.Clients
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
            => !HttpHelper.Post($"{_baseUrl}/api/HostManagement/Start/{appPoolName}",token).IsError;

        public bool StopAppPool(string appPoolName, string token)
            => !HttpHelper.Post($"{_baseUrl}/api/HostManagement/Stop/{appPoolName}", token).IsError;

        public bool AddAppPool(string appPoolName, string token)
            => !HttpHelper.Post($"{_baseUrl}/api/HostManagement/AddPool/{appPoolName}", token).IsError;

        public bool RemoveAppPool(string appPoolName, string token)
            => !HttpHelper.Post($"{_baseUrl}/api/HostManagement/RemovePool/{appPoolName}", token).IsError;

        public bool AddApp(string appName, string token)
            => !HttpHelper.Post($"{_baseUrl}/api/HostManagement/AddApp/{appName}", token).IsError;

        public bool RemoveApp(string appName, string token)
            => !HttpHelper.Post($"{_baseUrl}/api/HostManagement/RemoveApp/{appName}", token).IsError;

        public Dictionary<string, bool> GetAppPools(string token)
            => HttpHelper.Get<Dictionary<string,bool>>($"{_baseUrl}/api/HostManagement/AppPools", token).ThrowIfError();
        public List<string> GetUnreferencedDirectories(string token)
            => HttpHelper.Get<List<string>>($"{_baseUrl}/api/HostManagement/UnreferencedDirectories", token).ThrowIfError();

    }
}
