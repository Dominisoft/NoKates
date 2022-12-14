using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoKates.Common.Infrastructure.CustomExceptions;
using NoKates.Common.Infrastructure.Helpers;

namespace NoKates.WebAdmin.Pages
{
    public class MetricsModel : PageModel
    {

        public Dictionary<string, List<StatusPeriod>> StatusPeriods { get; set; }

        public string Good = "#2fcc66";
        public string Degraded = "#ffff00";
        public string Issue = "#ff9900";
        public string Offline = "#ff3333";
        public string NoTraffic = "#cccccc";

        public void OnGet()
        {
            var token = Request.Cookies["AccessToken"];
            if (string.IsNullOrWhiteSpace(token))
            {
                HttpContext.RedirectToRelativePath("Login");
                return;
            }

            FetchData(token);
        }
        public void FetchData(string token)
        {
            try
            {
                var metrics = HttpHelper.Get<List<ServicesModel.RequestMetricSummary>>($"{GlobalConfig.MetricsEndpointUrl}{Request.Query["ServiceName"]}/endpoints", token).Object;
                StatusPeriods = new Dictionary<string, List<StatusPeriod>>();
                var endpoints = metrics.Select(m => m.Name).Distinct().ToList();
                foreach (var endpoint in endpoints)
                {
                    var items = metrics.Where(m => m.Name == endpoint).ToList();
                    TryGeneratePeriods(items, endpoint);

                }
            }
            catch( Exception ex)
            {
                throw new RequestException(200, $"Unable to get metrics, make sure Metrics service is online and configured.\r\n\r\nError : {ex.Message}");
            }

        }
        public void TryGeneratePeriods(List<ServicesModel.RequestMetricSummary> metrics,string name)
        {

            try
            {
                var periods = metrics.Select(metric =>
                {
                    var status = Good;

                    var errorPercent =
                    metric.RequestCount == 0 ? 0 : (decimal)metric.Errors / (decimal)metric.RequestCount * 100;


                    if (metric.RequestCount == 0)
                        status = NoTraffic;
                    else if (errorPercent > GlobalConfig.ErrorPercentThreshold)
                        status = Issue;
                    else if (metric.AverageResponseTime > GlobalConfig.ResponseTimeThreshold)
                        status = Degraded;


                    return new StatusPeriod
                    {
                        Index = metric.Index,
                        X = (672) - metric.Index * 4,
                        Height = 50,
                        Width = 3,
                        Color = status,
                        RequestMetric = metric,
                        ErrorPercent = errorPercent
                    };
                }).ToList();
                StatusPeriods[name] = periods;
            }catch ( Exception ex)
            {
                StatusPeriods[name] = new List<StatusPeriod>
                {
                    new StatusPeriod
                    {
                        Index = 0,
                        Width = 168,
                        Height = 50,
                        Color = NoTraffic,
                        RequestMetric = new ServicesModel.RequestMetricSummary
                        {

                        }

                    }
                };
            }
        }

    }
}
