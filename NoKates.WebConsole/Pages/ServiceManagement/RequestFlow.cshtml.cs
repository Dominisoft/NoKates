using System;
using System.Collections.Generic;
using System.Linq;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;
using NoKates.LogsAndMetrics.Common;
using NoKates.WebConsole.Helpers;
using NoKates.WebConsole.Models;
using NoKates.WebConsole.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NoKates.WebConsole.Pages.ServiceManagement
{
    public class RequestFlowModel : AuthenticatedPage
    {
        private readonly IMetricsClient _metricsClient;
        public RequestFlowNode RequestFlow;

        public RequestFlowModel(IMetricsClient metricsClient)
        {
            _metricsClient = metricsClient;
            RequestFlow = new RequestFlowNode();
        }

        public override void LoadData(string token)
        {

            var requestId = Request.Query["requestId"];

            GetRequestFlow(requestId, token);

        }

        private void GetRequestFlow(string requestId, string token)
        {
            var requestMetrics = _metricsClient.GetMetricsByRequestId(Guid.Parse(requestId), token).ThrowIfError();

            RequestFlow = MapRequestFlow(requestMetrics);
        }


        private RequestFlowNode MapRequestFlow(List<RequestMetric> metrics)
        {
            var root = metrics.FirstOrDefault(m => m.RequestSource == "External" ||
                                                   m.RequestStart == metrics.Min(m2 => m2.RequestStart));

            var result = new RequestFlowNode
            {
                Request = root
                ,
                SubRequests = GetSubRequests(root?.ServiceName, metrics, 0)
            };

            return result;

        }

        private List<RequestFlowNode> GetSubRequests(string requestSource, List<RequestMetric> metrics, int level)
        {
            if (level > 8)
                return new List<RequestFlowNode>();

            var requests = metrics.Where(m => m.RequestSource == requestSource);
            if (!requests?.Any() ?? false) return new List<RequestFlowNode>();

            var newList = metrics.Where(r => !requests.Contains(r)).ToList();

            return requests.Select(request => new RequestFlowNode
            {
                Request = request,
                SubRequests = GetSubRequests(request.ServiceName, newList, level + 1)
            }).ToList();

        }
    }
}