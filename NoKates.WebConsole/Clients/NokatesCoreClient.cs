using System.Collections.Generic;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;
using NoKates.WebConsole.Extensions;
using Microsoft.Extensions.Primitives;

namespace NoKates.WebConsole.Clients
{
    public interface INoKatesCoreClient
    {
        Dictionary<string, List<string>> GetEndpointGroups(string token);
        List<ServiceStatus> GetServiceStatuses(string token);
        List<RequestMetric> GetRequestMetrics(string serviceName, string token);
        List<LogEntry> GetLogEntries(string serviceName,string token);
    }
    public class NoKatesCoreClient : INoKatesCoreClient
    {
        private readonly string _baseUrl;

        public NoKatesCoreClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }


        public Dictionary<string, List<string>> GetEndpointGroups(string token)
            => HttpHelper.Get<Dictionary<string, List<string>>>($"{_baseUrl}/EndpointGroups",token).ThrowIfError();

        public List<ServiceStatus> GetServiceStatuses(string token)
            => HttpHelper.Get<List<ServiceStatus>>($"{_baseUrl}/", token).ThrowIfError();

        public List<RequestMetric> GetRequestMetrics(string serviceName, string token)
            => HttpHelper.Get<List<RequestMetric>>($"{_baseUrl}/{serviceName}/NoKates/Requests", token)
                .ThrowIfError();

        public List<LogEntry> GetLogEntries(string serviceName, string token)
            => HttpHelper.Get<List<LogEntry>>($"{_baseUrl}/{serviceName}/NoKates/Log", token)
                .ThrowIfError();
    }
}
