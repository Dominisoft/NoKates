using System;
using System.Collections.Generic;
using System.Linq;
using NoKates.Common.Infrastructure.CustomExceptions;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.LogsAndMetrics.Common;
using NoKates.LogsAndMetrics.Common.DataTransfer;
using NoKates.WebConsole.Helpers;
using NoKates.WebConsole.Models;
using NoKates.WebConsole.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NoKates.WebConsole.Pages.ServiceManagement
{
    public class EndpointStatusListModel : AuthenticatedPage
    {
        private readonly IMetricsClient _metricsClient;

public Dictionary<string, List<StatusPeriod>> StatusPeriods { get; set; }

        public string Good = "#2fcc66";
        public string Degraded = "#ffff00";
        public string Issue = "#ff9900";
        public string Offline = "#ff3333";
        public string NoTraffic = "#cccccc";

        public EndpointStatusListModel(IMetricsClient metricsClient)
        {
            _metricsClient = metricsClient;
        }


        public override void LoadData(string token)
        {
            try
            {
                var serviceName = Request.Query["ServiceName"];
                var metrics = _metricsClient.GetEndpointMetricsSummaryByServiceName(serviceName).ThrowIfError();
                StatusPeriods = new Dictionary<string, List<StatusPeriod>>();
                var endpoints = metrics.Select(m => m.Name).Distinct().ToList();
                foreach (var endpoint in endpoints)
                {
                    var items = metrics.Where(m => m.Name == endpoint).ToList();
                    TryGeneratePeriods(items, endpoint);

                }
            }
            catch (Exception ex)
            {
                throw new RequestException(200, $"Unable to get metrics, make sure Metrics service is online and configured.\r\n\r\nError : {ex.Message}");
            }

        }
        public void TryGeneratePeriods(List<RequestMetricSummaryDto> metrics, string name)
        {

            try
            {
                var periods = metrics.Select(metric =>
                {
                    var status = Good;
                    var period = new StatusPeriod
                    {
                        Index = metric.Index,
                        X = (672) - metric.Index * 4,
                        Height = 50,
                        Width = 3,
                        Color = status,
                        RequestMetric = metric,
                    };
                    

                    if (metric.RequestCount == 0)
                        status = NoTraffic;
                    else if (period.ErrorPercent > GlobalConfiguration.ErrorPercentThreshold)
                        status = Issue;
                    else if (metric.AverageResponseTime > GlobalConfiguration.ResponseTimeThreshold)
                        status = Degraded;
                    period.Color = status;

                    return period;
                }).ToList();
                StatusPeriods[name] = periods;
            }
            catch (Exception ex)
            {
                StatusPeriods[name] = new List<StatusPeriod>
                {
                    new StatusPeriod
                    {
                        Index = 0,
                        Width = 168,
                        Height = 50,
                        Color = NoTraffic,
                        RequestMetric = new RequestMetricSummaryDto()
                        {

                        }

                    }
                };
            }
        }

    }
}