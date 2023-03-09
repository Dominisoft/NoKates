using NoKates.Common.Infrastructure.Configuration;
using NoKates.Common.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http.Extensions;
using Prometheus;
using Metrics = Prometheus.Metrics;

namespace NoKates.Common.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
       
        public static IApplicationBuilder UseNoKates(this IApplicationBuilder app)
        {
            //app.UseRouting();
            var logRequestMetrics = ConfigurationValues.GetBoolValueOrDefault("LogRequestMetrics");
            if (logRequestMetrics)
            app.UseMiddleware<RequestMetricsMiddleware>();
            app.UseAuthentication();
           // app.UseAuthorization();
            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseMiddleware<AuthorizationMiddleware>();

            return app;
        }

        private static IApplicationBuilder UsePrometheus(this IApplicationBuilder app)
        {


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
            });

            return app;
        }
        
    }
}
