using NoKates.Common.Infrastructure.Configuration;

namespace NoKates.Admin
{
    public class GlobalConfiguration
    {
        public static void LoadValuesFromNokates()
        {
            IdentityServiceUrl = ConfigurationValues.Values["IdentityServiceUrl"];
            RootEndpointUrl = ConfigurationValues.Values["BaseUrl"];
            LogsAndMetricsServiceUrl = ConfigurationValues.Values["LogsAndMetricsServiceUrl"];
            BaseUrl = ConfigurationValues.Values["BaseUrl"];
            HostSiteName = ConfigurationValues.Values["HostSiteName"];
            ConfigurationServiceUrl = ConfigurationValues.Values["ConfigurationServiceUrl"];
        }

        public static string IdentityServiceUrl { get; set; }
    
        public static string RootEndpointUrl { get; set; }
    public static string LogsAndMetricsServiceUrl { get; set; }
    public static int ResponseTimeThreshold { get; set; } = 100;
        public static int ErrorPercentThreshold { get; set; } = 5;
        public static string BaseUrl { get; set; }
    public static string HostSiteName { get; set; }
    public static string ConfigurationServiceUrl { get; set; }
    }
}
