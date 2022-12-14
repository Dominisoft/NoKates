using System;

namespace NoKates.LogsAndMetrics.Common.DataTransfer
{
    public class RequestMetricSummaryDto
    {
        public string Name { get; set; }
        public int RequestCount { get; set; }
        public DateTime FirstRequest { get; set; }
        public DateTime LastRequest { get; set; }
        public int Index { get; set; }
        public int AverageResponseTime { get; set; }
        public int Errors { get; set; }
    }
}
