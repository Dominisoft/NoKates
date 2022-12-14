using System;
using System.ComponentModel.DataAnnotations.Schema;
using NoKates.Common.Infrastructure.Attributes;

namespace NoKates.Common.Models
{
    [DefaultConnectionString("Metrics")]
    [Table("LogEntrys")]
    public class LogEntry : Entity
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }

    }
}
