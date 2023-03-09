using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.Common.Infrastructure.Client
{
    public interface INoKatesCoreClient
    {
        RestResponse<JObject> GetStartupConfig();
        RestResponse<ServiceStatus[]> GetAllServiceStatuses();
        RestResponse<Dictionary<string, List<string>>> GetEndpointGroups();
        RestResponse<ServiceStatus[]> GetAllServiceStatuses(string authToken);
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

        public RestResponse<Dictionary<string, List<string>>> GetEndpointGroups()
         => GetEndpointGroups(_authToken);

        public RestResponse<JObject> GetStartupConfig()
            => HttpHelper.Get<JObject>($"{_baseUrl}/Startup");

        public RestResponse<ServiceStatus[]> GetAllServiceStatuses(string authToken)
            => HttpHelper.Get<ServiceStatus[]>($"{_baseUrl}/", authToken);

        public RestResponse<Dictionary<string, List<string>>> GetEndpointGroups(string authToken)
            => HttpHelper.Get<Dictionary<string, List<string>>>($"{_baseUrl}/EndpointGroups", authToken);
    }
}
