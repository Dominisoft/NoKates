using NoKates.Common.Infrastructure.Controllers;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace NoKates.LogsAndMetrics.Controllers
{
    [Route("[controller]")]

    public class LogEntryController : BaseController<LogEntry>
    {
        public LogEntryController() : base(RepositoryHelper.CreateRepository<LogEntry>())
        { }
    }
}
