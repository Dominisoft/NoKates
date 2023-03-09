using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NoKates.Common.Infrastructure.Configuration;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Identity.Common.Clients;
using NoKates.LogsAndMetrics.Common;
using NoKates.WebConsole.Application;
using NoKates.WebConsole.Clients;
using AuthenticationClient = NoKates.Identity.Common.Clients.AuthenticationClient;

namespace NoKates.WebConsole
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var nam = Assembly.GetEntryAssembly()?.GetName();
            var version = nam.Version;
            services.AddNoKates()
                .AddSingleton<IRoleClient>(new RoleClient(GlobalConfiguration.IdentityServiceUrl))
                .AddSingleton<IUserClient>(new UserClient(GlobalConfiguration.IdentityServiceUrl))
                .AddSingleton<IAuthenticationClient>(new AuthenticationClient(GlobalConfiguration.IdentityServiceUrl))
                .AddSingleton<INoKatesCoreClient>(new NoKatesCoreClient(GlobalConfiguration.RootEndpointUrl))
                .AddSingleton<IMetricsClient>(new MetricsClient(GlobalConfiguration.LogsAndMetricsServiceUrl))
                .AddSingleton<IWebHostManagementService>(new WebHostManagementService())
                .AddSingleton<IWebHostManagementClient>(new WebHostManagementClient(GlobalConfiguration.BaseUrl+"/"+AppHelper.GetAppName()))
                .AddSingleton<IConfigurationClient>(new ConfigurationClient(GlobalConfiguration.ConfigurationServiceUrl))
                ;
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseNoKates();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            ConfigureThresholds();
        }

        private void ConfigureThresholds()
        {
            if (ConfigurationValues.TryGetValue<int>(out var ResponseTimeThreshold, "ResponseTimeThreshold"))
                GlobalConfiguration.ResponseTimeThreshold = ResponseTimeThreshold;

            if (ConfigurationValues.TryGetValue<int>(out var ErrorPercentThreshold, "ErrorPercentThreshold"))
                GlobalConfiguration.ErrorPercentThreshold = ErrorPercentThreshold;



        }
    }
}
