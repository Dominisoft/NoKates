using System;
using NoKates.Common.Models;

namespace NoKates.LogsAndMetrics.Common.DataTransfer
{
    public class LogEntryDto : Entity
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }

    }
}
