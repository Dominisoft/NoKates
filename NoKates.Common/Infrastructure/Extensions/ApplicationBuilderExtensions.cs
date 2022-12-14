using NoKates.Common.Infrastructure.Configuration;
using NoKates.Common.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

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

        
    }
}
