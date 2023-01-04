using System;
using System.Collections.Generic;
using System.Linq;
using NoKates.Common.Infrastructure.CustomExceptions;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;
using NoKates.LogsAndMetrics.Common;
using NoKates.LogsAndMetrics.Common.DataTransfer;
using NoKates.WebConsole.Clients;
using NoKates.WebConsole.Extensions;
using NoKates.WebConsole.Helpers;
using NoKates.WebConsole.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NoKates.WebConsole.Pages.ServiceManagement
{
    public class StatusListModel : PageModel
    {
        private readonly IMetricsClient _metricsClient;
        private readonly INoKatesCoreClient _adminClient;
        private readonly IWebHostManagementClient _webHostManagementClient;

        public Dictionary<string, bool> AppStatus;

        public List<ServiceStatus> ServiceStatuses { get; set; }
        public Dictionary<string, List<StatusPeriod>> StatusPeriods { get; set; }

        public string Good = "#2fcc66";
        public string Degraded = "#ffff00";
        public string Issue = "#ff9900";
        public string Offline = "#ff3333";
        public string NoTraffic = "#cccccc";

        public StatusListModel(IMetricsClient metricsClient, INoKatesCoreClient adminClient, IWebHostManagementClient webHostManagementClient)
        {
            _metricsClient = metricsClient;
            _adminClient = adminClient;
            _webHostManagementClient = webHostManagementClient;
            ServiceStatuses = new List<ServiceStatus>();
            StatusPeriods = new Dictionary<string, List<StatusPeriod>>();
            AppStatus = new Dictionary<string, bool>();
        }

        public void LoadData(string token)
        {

            try
            {
                AppStatus = _webHostManagementClient.GetAppPools(token)??new Dictionary<string, bool>();

                ServiceStatuses = _adminClient.GetServiceStatuses(token).OrderBy(s => $"{s?.IsOnline}{s?.Name}")?.ToList();
                ServiceStatuses.ForEach(status =>
                {
                    if (status?.IsOnline ?? false)
                        TryGeneratePeriods(status.Name, token);
                });


            }
            catch (Exception ex)
            {
                throw new RequestException(200,
                    $"Unable to Generate Charts for Service Statuses, Check service host\r\n\r\nError : {ex.Message} \r\n{ex.StackTrace}");
            }

        }

        public void TryGeneratePeriods(string name, string token)
        {
            try
            {
                HttpHelper.SetToken(token);
                var metrics = _metricsClient.GetMetricSummaryByServiceName(name).ThrowIfError();
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
                        RequestMetric = metric

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



        public  void OnGet()
        {
            var token = Request.GetToken();
            if (TokenHelper.TokenIsInvalid(token))
            {
                HttpContext.RedirectToRelativePath("Session/Login");
                return;
            }

            var function = Request.Query["function"];
            if (string.IsNullOrWhiteSpace(function))
            {
                LoadData(token);
                return;
            }
          


            var name = Request.Query["name"];
            switch (function)
            {
                case nameof(StartAppPool):
                    StartAppPool(name);
                    break;
                case nameof(StopAppPool):
                    StopAppPool(name);
                    break;
                case nameof(AddApp):
                    AddApp(name);
                    break;
                case nameof(RemoveApp):
                    RemoveApp(name);
                    break;
                case nameof(AddAppPool):
                    AddAppPool(name);
                    break;
                case nameof(RemoveAppPool):
                    RemoveAppPool(name);
                    break;
            }
            Response.Redirect("./StatusList");

        }


        public void StartAppPool(string appPoolName)
        {
            var token = Request.GetToken();
            _webHostManagementClient.StartAppPool(appPoolName, token);
            LoadData(token);
        }
        public void StopAppPool(string appPoolName)
        {
            var token = Request.GetToken();
            _webHostManagementClient.StopAppPool(appPoolName, token);
            LoadData(token);
        }
        public void AddAppPool(string appPoolName)
        {
            var token = Request.GetToken();
            _webHostManagementClient.AddAppPool(appPoolName, token);
            LoadData(token);
        }
        public void RemoveAppPool(string appPoolName)
        {
            var token = Request.GetToken();
            _webHostManagementClient.RemoveAppPool(appPoolName, token);
            LoadData(token);
        }
        public void AddApp(string appPoolName)
        {
            var token = Request.GetToken();
            _webHostManagementClient.AddApp(appPoolName, token);
            LoadData(token);
        }
        public void RemoveApp(string appPoolName)
        {
            var token = Request.GetToken();
            _webHostManagementClient.RemoveApp(appPoolName, token);
            LoadData(token);
        }


    }
}