using System;
using System.Collections.ObjectModel;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.Common.Infrastructure.Configuration
{
    public static class StatusValues
    {
        private static string _source = AppHelper.GetAppName();
        public static ServiceStatus Status;
        public static ObservableCollection<LogEntry> EventLog = new ObservableCollection<LogEntry>();
        public static ObservableCollection<RequestMetric> RequestMetrics = new ObservableCollection<RequestMetric>();
        public static ObservableCollection<RepositoryMetric> RepositoryMetrics = new ObservableCollection<RepositoryMetric>();
        internal static void Log(string str)
        {
            var s = RedactSensitiveInfo(str);
            EventLog.Add( new LogEntry
            {
                Date = DateTime.Now,
                Message = s,
                Source = _source
            });
            Console.WriteLine(s);
        }

        private static string RedactSensitiveInfo(string str)
        {
    //        var secretKeys = ConfigurationValues.Values?.Keys?.Where(key => key?.ToLower()?.ContainsOneOf("pass", "secret","connection")??false).ToList()??new List<string>();
    //        return secretKeys.Aggregate(str, (current, key) => current.Replace(ConfigurationValues.Values[key], "**********"));
    return str;

        }

        internal static void LogRequestAndResponse(RequestMetric request)
        {
            var path = request.RequestPath;
            if (path.ToLower().StartsWith("/nokates")) return;
            RequestMetrics.Add(request);
        }

        public static void LogRepositoryMetric(RepositoryMetric metric)
            => RepositoryMetrics.Add(metric);
    }
}
