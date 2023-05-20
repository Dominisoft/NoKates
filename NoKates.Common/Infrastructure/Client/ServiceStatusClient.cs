using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;
namespace NoKates.Common.Infrastructure.Client
{
    public interface IServiceStatusClient
    {
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
        public ServiceStatusClient(string baseUrl, string token):this(baseUrl)
        {
            _token = token;
        }

        public ServiceStatusClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }


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
            => HttpHelper.Get<ServiceStatus>($"{_baseUrl}/NoKates/ServiceStatus",token);


        public RestResponse<List<LogEntry>> GetLog(string token)
            => HttpHelper.Get<List<LogEntry>>($"{_baseUrl}/NoKates/Log", token);


        public RestResponse<List<RequestMetric>> GetRequestResponses(string token)
            => HttpHelper.Get<List<RequestMetric>>($"{_baseUrl}/NoKates/Requests", token);


        public RestResponse<Dictionary<string, List<string>>> GetEndpointGroups(string token)
            => HttpHelper.Get<Dictionary<string, List<string>>>($"{_baseUrl}/NoKates/EndpointGroups", token);


        public RestResponse<List<EndpointDescription>> ListAllEndpoints(string token)
            => HttpHelper.Get<List<EndpointDescription>>($"{_baseUrl}/NoKates/Endpoints", token);

    }
}
