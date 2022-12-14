using NoKates.Common.Infrastructure.Configuration;

namespace NoKates.WebConsole
{
    public class GlobalConfiguration
    {
        public static string IdentityServiceUrl => ConfigurationValues.Values["IdentityServiceUrl"];
        public static string RootEndpointUrl => ConfigurationValues.Values["RootEndpointUrl"];
        public static string LogsAndMetricsServiceUrl => ConfigurationValues.Values["LogsAndMetricsServiceUrl"];
        public static int ResponseTimeThreshold { get; set; } = 100;
        public static int ErrorPercentThreshold { get; set; } = 5;
        public static string BaseUrl => ConfigurationValues.Values["BaseUrl"];
        public static string HostSiteName => ConfigurationValues.Values["HostSiteName"];
    }
}
