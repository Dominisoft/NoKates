using NoKates.Core.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NoKates.Common.Infrastructure.Helpers;

namespace NoKates.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceStatusHelper.SiteName = AppHelper.GetAppName();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
