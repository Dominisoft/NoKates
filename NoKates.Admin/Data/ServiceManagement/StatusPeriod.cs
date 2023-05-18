using NoKates.LogsAndMetrics.Common.DataTransfer;

namespace NoKates.Admin.Data.ServiceManagement;
public class StatusPeriod
{
    public int Index { get; set; }
    public string Color { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public decimal ErrorPercent => (RequestMetric?.RequestCount ?? 0) == 0 ? 0 : (decimal)RequestMetric.Errors / (decimal)RequestMetric.RequestCount * 100;
    public RequestMetricSummaryDto RequestMetric { get; set; }
}