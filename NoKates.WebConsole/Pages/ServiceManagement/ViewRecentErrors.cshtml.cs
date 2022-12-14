using System;
using System.Collections.Generic;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Models;
using NoKates.LogsAndMetrics.Common;
using NoKates.WebConsole.Clients;
using NoKates.WebConsole.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NoKates.WebConsole.Pages.ServiceManagement
{
    public class ViewRecentErrorsModel : AuthenticatedPage
    {
        private readonly IMetricsClient _metricsClient;
        public List<RequestMetric> LogEntries;
        public List<RequestMetric> FilteredRequestMetrics;


        public string RequestTrackingId { get; set; }
        public string ResponseCode { get; set; }
        public string Source { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Contains { get; set; }
        public string NotContains { get; set; }
        public string ServiceName { get; set; }
        public ViewRecentErrorsModel(IMetricsClient metricsClient)
        {
            _metricsClient = metricsClient;
            LogEntries = new List<RequestMetric>();
            FilteredRequestMetrics = new List<RequestMetric>();
        }
        public override void LoadData(string token)
        {


            RequestTrackingId = GetQueryString(nameof(RequestTrackingId));
            ResponseCode = GetQueryString(nameof(ResponseCode));
            Source = GetQueryString(nameof(Source));
            Start = GetQueryString(nameof(Start));
            End = GetQueryString(nameof(End));
            Contains = GetQueryString(nameof(Contains));
            NotContains = GetQueryString(nameof(NotContains));
            ServiceName = GetQueryString(nameof(ServiceName));



            LogEntries = _metricsClient.GetRecentErrors(token).ThrowIfError();

            Filter();
        }

        private string GetQueryString(string name)
        {
            var value = string.Empty;
            if (Request.Query.ContainsKey(name))
                value = Request.Query[name];
            return value;
        }

        public string GetExpanded(Guid? entryRequestTrackingId)
        {
            return entryRequestTrackingId?.ToString() == RequestTrackingId ? "Open" : string.Empty;
        }





        public void Filter()
        {

            FilteredRequestMetrics = LogEntries ?? new List<RequestMetric>();

            if (!string.IsNullOrWhiteSpace(Source))
                FilteredRequestMetrics.RemoveAll(e => !e.RequestSource.Contains(Source));
            if (!string.IsNullOrWhiteSpace(RequestTrackingId))
                FilteredRequestMetrics.RemoveAll(e => e.RequestTrackingId.ToString() != RequestTrackingId);
            if (!string.IsNullOrWhiteSpace(ResponseCode) && ResponseCode != "0")
                FilteredRequestMetrics.RemoveAll(e => e.ResponseCode.ToString() != ResponseCode);
            if (!string.IsNullOrWhiteSpace(Contains))
                FilteredRequestMetrics.RemoveAll(e => !e.Serialize().Contains(Contains));
            if (!string.IsNullOrWhiteSpace(NotContains))
                FilteredRequestMetrics.RemoveAll(e => e.Serialize().Contains(NotContains));

            if (!string.IsNullOrWhiteSpace(ServiceName))
                FilteredRequestMetrics.RemoveAll(e => !e.ServiceName.Contains(ServiceName));
            if (!string.IsNullOrWhiteSpace(Start))
                FilteredRequestMetrics.RemoveAll(e => e.RequestStart < DateTime.Parse(Start));
            if (!string.IsNullOrWhiteSpace(End))
                FilteredRequestMetrics.RemoveAll(e => e.RequestStart > DateTime.Parse(End));





        }

    }
}