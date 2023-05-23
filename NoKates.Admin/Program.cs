using System.Text.Json;
using Blazored.LocalStorage;
using Blazored.Toast;
using NoKates.Admin;
using NoKates.Admin.Clients;
using NoKates.Common.Infrastructure.Client;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Identity.Common.Clients;
using NoKates.LogsAndMetrics.Common;
using AuthenticationClient = NoKates.Identity.Common.Clients.AuthenticationClient;
using INoKatesCoreClient = NoKates.Common.Infrastructure.Client.INoKatesCoreClient;
using NoKatesCoreClient = NoKates.Common.Infrastructure.Client.NoKatesCoreClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddBlazoredLocalStorage(config => {
        config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        config.JsonSerializerOptions.IgnoreNullValues = true;
        config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
        config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
        config.JsonSerializerOptions.WriteIndented = false;
    }
);
builder.Services.AddBlazoredToast();

AppHelper.SetAppName("Management");

builder.Services.AddNoKates();
GlobalConfiguration.LoadValuesFromNokates();
var baseUrl = GlobalConfiguration.BaseUrl;
builder.Services.AddSingleton<INoKatesCoreClient>(new NoKatesCoreClient(GlobalConfiguration.BaseUrl));
builder.Services.AddSingleton<IMetricsClient>(new MetricsClient(GlobalConfiguration.LogsAndMetricsServiceUrl));
builder.Services.AddSingleton<IAuthenticationClient>(new AuthenticationClient(GlobalConfiguration.IdentityServiceUrl));
builder.Services.AddSingleton<IConfigurationClient>(new ConfigurationClient(GlobalConfiguration.ConfigurationServiceUrl));
builder.Services.AddSingleton<IWebHostManagementClient>(new WebHostManagementClient($"{baseUrl}/{AppHelper.GetAppName()}"));
builder.Services.AddTransient<IServiceStatusClient>((x) => new ServiceStatusClient(baseUrl, ""));
//var mock = new Mock<INoKatesCoreClient>();

//var apps = new List<string> {"My.TestApp"};
//mock.Setup(x => x.GetAllServiceNames(It.IsAny<string>())).Returns(new RestResponse<string[]>(200,""){Object = apps.ToArray()});
//builder.Services.AddSingleton(mock.Object);



//var metrics = new List<RequestMetricSummaryDto>();
//var m2 = new Mock<IMetricsClient>();
//m2.Setup(x => x.GetMetricSummaryByServiceName(It.IsAny<string>())).Returns(new RestResponse<List<RequestMetricSummaryDto>>(200,""){Object = metrics });
//builder.Services.AddSingleton(m2.Object);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.UseNoKates();


app.Run();
