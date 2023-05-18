using Moq;
using NoKates.Common.Infrastructure.Client;
using NoKates.Common.Models;
using NoKates.LogsAndMetrics.Common;
using NoKates.LogsAndMetrics.Common.DataTransfer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var core = new NoKatesCoreClient("http://LocalServiceHost/");
var metrics = new MetricsClient("http://LocalServiceHost/NoKates.LogsAndMetrics/");
builder.Services.AddSingleton<INoKatesCoreClient>(core);
builder.Services.AddSingleton<IMetricsClient>(metrics);


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
