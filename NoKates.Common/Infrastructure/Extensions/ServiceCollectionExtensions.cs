using System;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using NoKates.Common.Infrastructure.Configuration;
using NoKates.Common.Infrastructure.Controllers;
using NoKates.Common.Infrastructure.Conventions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Infrastructure.Listener;
using NoKates.Common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RepoDb;
using RepoDb.DbHelpers;
using RepoDb.DbSettings;
using RepoDb.StatementBuilders;

namespace NoKates.Common.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNoKates(this IServiceCollection services, string configurationAppName = "Configuration", string configFile = null)
        {
            SetupRepoDb();

            if (string.IsNullOrWhiteSpace(configFile))
            {
                ConfigurationValues.LoadConfig(configurationAppName);
            }
            else
            {
                ConfigurationValues.LoadConfigFromFile(configFile);
            }
            EventRecorder.Start();

            services.SetupJwtServices();

            var mvc = services.AddMvcCore(options =>
            {
                options.Conventions.Add(new ApiExplorerVisibilityEnabledConvention());
            }
            );

            mvc.AddApplicationPart(typeof(StatusController).Assembly);
            StatusValues.Status = new ServiceStatus
            {
                IsOnline = true,
                StartTime = DateTime.Now,
                Name = AppHelper.GetAppName(),
                Uri = AppHelper.GetAppUri() + "ServiceStatus",
                Version = AppHelper.GetVersionDetails(),
                DeploymentStatus = DeploymentStatus.DeploymentCompleted
            };
            StatusValues.Log("NoKates Service configured");


            return services;
        }
        private static void SetupRepoDb()
        {
            SqlServerBootstrap.Initialize();

            var dbSetting = new SqlServerDbSetting();

            DbSettingMapper
                .Add<SqlConnection>(dbSetting, true);
            DbHelperMapper
                .Add<SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper
                .Add<SqlConnection>(new SqlServerStatementBuilder(dbSetting), true);
        }
        private static void SetupJwtServices(this IServiceCollection services)
        {
            //TODO: This should come from config
            var key = "my_secret_key_12345"; //this should be same which is used while creating token    
            var issuer = "http://mysite.com";  //this should be same which is used while creating token


            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = issuer,
                        ValidAudience = issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            //services.AddAuthorization();
        }
    }
}
