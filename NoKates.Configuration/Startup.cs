using NoKates.Configuration.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NoKates.Common.Infrastructure.Configuration;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Configuration.Application;

namespace NoKates.Configuration
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        private string ConfigurationDirectory => ConfigurationValues.Values["ConfigurationDirectory"];

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNoKates(configFile: "./config.json");

            services.AddSingleton<IConfigReader>(new ConfigReader(ConfigurationDirectory));
            services.AddTransient<IConfigurationService, ConfigurationService>();
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
            app.UseMiddleware<LocalOnlyMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
