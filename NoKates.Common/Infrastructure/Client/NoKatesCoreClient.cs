using System.Collections.Generic;
using Microsoft.Web.Administration;
using Newtonsoft.Json.Linq;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.Common.Infrastructure.Client
{
    public interface INoKatesCoreClient
    {
        RestResponse<JObject> GetStartupConfig();
        RestResponse<ServiceStatus[]> GetAllServiceStatuses();
        RestResponse<string[]> GetAllServiceNames();
        RestResponse<PoolState> GetPoolState(string name);

        RestResponse<Dictionary<string, List<string>>> GetEndpointGroups();
        RestResponse<ServiceStatus[]> GetAllServiceStatuses(string authToken);
        RestResponse<PoolState> GetPoolState(string name, string authToken);
        RestResponse<string[]> GetAllServiceNames(string authToken);
        RestResponse<Dictionary<string, List<string>>> GetEndpointGroups(string authToken);


    }
    public class NoKatesCoreClient : INoKatesCoreClient
    {
        private readonly string _baseUrl;
        private readonly string _authToken;

        public NoKatesCoreClient(string baseUrl, string authToken="")
        {
            _baseUrl = baseUrl;
            _authToken = authToken;
        }


        public RestResponse<ServiceStatus[]> GetAllServiceStatuses()
            => GetAllServiceStatuses(_authToken);

        public RestResponse<string[]> GetAllServiceNames()
            => GetAllServiceNames(_authToken);

        public RestResponse<PoolState> GetPoolState(string name)
            => GetPoolState(name, _authToken);
        public RestResponse<Dictionary<string, List<string>>> GetEndpointGroups()
         => GetEndpointGroups(_authToken);

        public RestResponse<JObject> GetStartupConfig()
            => HttpHelper.Get<JObject>($"{_baseUrl}/Startup");

        public RestResponse<ServiceStatus[]> GetAllServiceStatuses(string authToken)
            => HttpHelper.Get<ServiceStatus[]>($"{_baseUrl}/", authToken);
        public RestResponse<string[]> GetAllServiceNames(string authToken)
            => HttpHelper.Get<string[]>($"{_baseUrl}/Services", authToken);
        public RestResponse<PoolState> GetPoolState(string name, string authToken)
            => HttpHelper.Get<PoolState>($"{_baseUrl}/Pool/{name}", authToken);

        public RestResponse<Dictionary<string, List<string>>> GetEndpointGroups(string authToken)
            => HttpHelper.Get<Dictionary<string, List<string>>>($"{_baseUrl}/EndpointGroups", authToken);

    }
}
