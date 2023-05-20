using System.Collections.Generic;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;
namespace NoKates.Common.Infrastructure.Client
{
    public interface IServiceStatusClient
    {
        void ChangeService(string service);
        RestResponse<ServiceStatus> GetStatus();
        RestResponse<List<LogEntry>> GetLog();
        RestResponse<List<RequestMetric>> GetRequestResponses();
        RestResponse<Dictionary<string, List<string>>> GetEndpointGroups();
        RestResponse<List<EndpointDescription>> ListAllEndpoints();
        RestResponse<ServiceStatus> GetStatus(string token);
        RestResponse<List<LogEntry>> GetLog(string token);
        RestResponse<List<RequestMetric>> GetRequestResponses(string token);
        RestResponse<Dictionary<string, List<string>>> GetEndpointGroups(string token);
        RestResponse<List<EndpointDescription>> ListAllEndpoints(string token);

    }
    public class ServiceStatusClient: IServiceStatusClient
    {
        private string _token;
        private string _baseUrl;
        private string _service;
        public ServiceStatusClient(string baseUrl,string service, string token):this(baseUrl,service)
        {
            _token = token;
        }

        public ServiceStatusClient(string baseUrl,string service)
        {
            _baseUrl = baseUrl.Trim('/');
            ChangeService(service);
        }

        public void ChangeService(string service)
            => _service = service.Trim('/');
        public RestResponse<ServiceStatus> GetStatus()
            => GetStatus(_token);

        public RestResponse<List<LogEntry>> GetLog()
            => GetLog(_token);

        public RestResponse<List<RequestMetric>> GetRequestResponses()
            => GetRequestResponses(_token);

        public RestResponse<Dictionary<string, List<string>>> GetEndpointGroups()
            => GetEndpointGroups(_token);

        public RestResponse<List<EndpointDescription>> ListAllEndpoints()
            => ListAllEndpoints(_token);

        public RestResponse<ServiceStatus> GetStatus(string token)
            => HttpHelper.Get<ServiceStatus>($"{_baseUrl}/{_service}/NoKates/ServiceStatus",token);


        public RestResponse<List<LogEntry>> GetLog(string token)
            => HttpHelper.Get<List<LogEntry>>($"{_baseUrl}/{_service}/NoKates/Log", token);


        public RestResponse<List<RequestMetric>> GetRequestResponses(string token)
            => HttpHelper.Get<List<RequestMetric>>($"{_baseUrl}/{_service}/NoKates/Requests", token);


        public RestResponse<Dictionary<string, List<string>>> GetEndpointGroups(string token)
            => HttpHelper.Get<Dictionary<string, List<string>>>($"{_baseUrl}/{_service}/NoKates/EndpointGroups", token);


        public RestResponse<List<EndpointDescription>> ListAllEndpoints(string token)
            => HttpHelper.Get<List<EndpointDescription>>($"{_baseUrl}/{_service}/NoKates/Endpoints", token);

    }
}
