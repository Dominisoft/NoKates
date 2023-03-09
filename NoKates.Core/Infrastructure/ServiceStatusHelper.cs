using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.Administration;
using NoKates.Common.Infrastructure.Configuration;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.Core.Infrastructure
{
    public static class ServiceStatusHelper
    {
        public static string SiteName;
        public static List<ServiceStatus> GetServices(string root)
        {
            var paths = GetApplicationStatusPagePaths(root);
            return paths.Select(GetStatus).ToList();
        }


        public static ServiceStatus GetStatus(string path)
        {
            var name = "";
            try
            {
                var token = ConfigurationValues.Token;
                StatusValues.EventLog.Add(new LogEntry
                {
                    Date = DateTime.Now,
                    Message = $"Got Token: {token?.Split('.').Length==3}",
                    Source = AppHelper.GetAppName()
                });
                
                var parts = path.Split("/").ToList();
                var index = parts.IndexOf("NoKates") - 1;
                name = parts[index];
                var result = Get<ServiceStatus>(path);
                result.Uri = path;
                if (result.Name == null)
                    throw new Exception("Failed to get service status");
                return result;
            }
            catch (Exception e)
            {
                StatusValues.EventLog.Add(new LogEntry
                {
                    Date = DateTime.Now,
                    Message = e.Message+"\r\n"+e.StackTrace,
                    Source = "NoKates.Core"
                });
                Console.WriteLine(e);
            }


            return new ServiceStatus
            {
                IsOnline = false,
                Name = name,
                Uri = path,
            };
        }

        public static Dictionary<string, List<string>> GetGroups(string root)
        {
            var paths = GetApplicationEndpointGroupsPagePaths(root);
            var groups = paths.Select(Get<Dictionary<string, List<string>>>).ToArray();
            return new Dictionary<string, List<string>>().UnionValues(groups);
        }

        private static TReturn Get<TReturn>(string path) where TReturn : class,new() 
            => HttpHelper.Get<TReturn>(path,ConfigurationValues.Token).Object??new TReturn();

        internal static List<string> GetApplicationStatusPagePaths(string root)
        {
            var appPaths = GetServicePaths();

            return appPaths.Select(path => $"{root}{path}/NoKates/ServiceStatus").ToList();

        }
        internal static List<string> GetApplicationEndpointGroupsPagePaths(string root)
        {
            var appPaths = GetServicePaths();

            return appPaths.Select(path => $"{root}{path}/NoKates/EndpointGroups").ToList();

        }
        private static string GetSiteName()
        {
            var serverManager = new ServerManager();
            var sites = serverManager.Sites;
            var maxCount = sites.Select(s => s.Applications.Count).Max();
            var site = sites.FirstOrDefault(s => s.Applications.Count == maxCount);
            return site?.Name;

        }
        internal static string[] GetServicePaths()
        {
            var apps = AppHelper.GetApps();
            var rootName = AppHelper.GetAppName();
            return apps.Where(app => app.ApplicationPoolName !=rootName).Select(app => app.ApplicationPoolName).ToArray();
        }

        public static List<string> GetServiceNames()
        {
            var apps = AppHelper.GetApps();
            return apps.Select(a => a.ApplicationPoolName).ToList();
        }
    }
}
