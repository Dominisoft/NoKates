using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.WebAdmin.Pages
{
    public class RequestLogsModel : PageModel
    {
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
        public RequestLogsModel()
        {
            LogEntries = new List<RequestMetric>();
        }
        public void OnGet()
        {
            var token = Request.Cookies["AccessToken"];
            var isValid = token?.Split('.')?.Length == 3;
            if (!isValid)
            {
                HttpContext.RedirectToRelativePath("Login");
                return;
            }

            RequestTrackingId = GetQueryString(nameof(RequestTrackingId));
            ResponseCode = GetQueryString(nameof(ResponseCode));
            Source = GetQueryString(nameof(Source));
            Start = GetQueryString(nameof(Start));
            End = GetQueryString(nameof(End));
            Contains = GetQueryString(nameof(Contains));
            NotContains = GetQueryString(nameof(NotContains));
            ServiceName = GetQueryString(nameof(ServiceName));

            var path = $"{Request.Scheme}://{Request.Host}";
            path = $"http://localserver.wardkraft.com/{ServiceName}/NoKates/Requests";

            
            LogEntries = TryGetLogEntries(path,token);

            Filter();
        }

        private string GetQueryString(string name)
        {
            var value = string.Empty;
            if (Request.Query.ContainsKey(name))
                value = Request.Query[name];
            return value;
        }

        private List<RequestMetric> TryGetLogEntries(string path,string token)
        {
            try
            {
                return HttpHelper.Get<List<RequestMetric>>(path, token);
            }
            catch (Exception e)
            {
                LoggingHelper.LogMessage(e.Message+"\r\n"+e.StackTrace);
                return new List<RequestMetric>() { new RequestMetric { RequestStart = DateTime.Now,RequestJson = "Unable to get Log Entries", ResponseJson = ""} };

            }

            return null;
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
            if (!string.IsNullOrWhiteSpace(ResponseCode) && ResponseCode!="0")
                FilteredRequestMetrics.RemoveAll(e => e.ResponseCode.ToString() != ResponseCode);
            if (!string.IsNullOrWhiteSpace(Contains))
                FilteredRequestMetrics.RemoveAll(e => !e.Serialize().Contains(Contains));
            if (!string.IsNullOrWhiteSpace(NotContains))
                FilteredRequestMetrics.RemoveAll(e => e.Serialize().Contains(NotContains));

            if (!string.IsNullOrWhiteSpace(Start))
                FilteredRequestMetrics.RemoveAll(e => e.RequestStart < DateTime.Parse(Start));
            if (!string.IsNullOrWhiteSpace(End))
                FilteredRequestMetrics.RemoveAll(e => e.RequestStart > DateTime.Parse(End));





        }

    }
}
