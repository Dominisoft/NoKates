using System;
using NoKates.Common.Models;

namespace NoKates.LogsAndMetrics.Models
{
    public class LogEntry : Entity
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }

    }
}
