using NoKates.Common.Infrastructure.Configuration;

namespace NoKates.WebAdmin
{
    public static class GlobalConfig
    {
        public static string IdentityServiceUrl => ConfigurationValues.Values["IdentityServiceUrl"];
        public static string EndpointGroupsUrl => ConfigurationValues.Values["EndpointGroupsUrl"];
        public static string RequestMetricUrl => ConfigurationValues.Values["RequestMetricUrl"];
        public static string RootEndpointUrl => ConfigurationValues.Values["RootEndpointUrl"];
        public static string MetricsEndpointUrl => ConfigurationValues.Values["MetricsEndpointUrl"];
        public static int ResponseTimeThreshold { get; set; } = 100;
        public static int ErrorPercentThreshold { get; set; } = 5;
        public static string AuthenticationUrl => ConfigurationValues.Values["AuthenticationUrl"];
        public static string RoleEndpointUrl => ConfigurationValues.Values["RoleEndpointUrl"];
        public static string UserEndpointUrl => ConfigurationValues.Values["UserEndpointUrl"];
    }
}
