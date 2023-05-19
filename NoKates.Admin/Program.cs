using System.Text.Json;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using NoKates.Admin.Clients;
using NoKates.Common.Infrastructure.Client;
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

var core = new NoKatesCoreClient("http://LocalServiceHost");
var metrics = new MetricsClient("http://LocalServiceHost/NoKates.LogsAndMetrics");
var identity = new AuthenticationClient("http://LocalServiceHost/NoKates.Identity");
var config = new ConfigurationClient("http://LocalServiceHost/NoKates.Configuration");
var webclient = new WebHostManagementClient("http://LocalServiceHost/Management");
builder.Services.AddSingleton<INoKatesCoreClient>(core);
builder.Services.AddSingleton<IMetricsClient>(metrics);
builder.Services.AddSingleton<IAuthenticationClient>(identity);
builder.Services.AddSingleton<IConfigurationClient>(config);
builder.Services.AddSingleton<IWebHostManagementClient>(webclient);

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



app.Run();
