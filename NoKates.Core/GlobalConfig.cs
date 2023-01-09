using NoKates.Common.Infrastructure.Configuration;

namespace NoKates.Core
{
    public static class GlobalConfig
    {
        public static string StartupConfig => ConfigurationValues.Values[nameof(StartupConfig)];
        public static string IconPath => ConfigurationValues.Values[nameof(IconPath)];
    }
}
