using NoKates.Common.Infrastructure.Configuration;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Repositories;
using NoKates.Common.Infrastructure.RepositoryConnections;
using NoKates.LogsAndMetrics.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NoKates.LogsAndMetrics
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
            services.AddNoKates()
                .AddScoped<IConnectionString,DefaultConnectionString>()
                .AddScoped<IMetricsManagementService,MetricsManagementService>()
                .AddTransient<ISqlRepository<Models.RequestMetric>>(s => s.GetService<IMetricsManagementService>());

            int.TryParse(ConfigurationValues.Values["PollInterval"], out var pollInterval);
            var username = ConfigurationValues.Values["ServiceAccountUsername"];
            var password= ConfigurationValues.Values["ServiceAccountPassword"];
            var rootUrl= ConfigurationValues.Values["BaseUrl"];
            var poll = new ServiceStatusPoller(pollInterval, username, password, rootUrl);
            services.AddSingleton(poll);
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
            app.UseNoKates();

            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
