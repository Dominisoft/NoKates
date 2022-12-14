using System.Collections.Generic;
using System.Linq;
using NoKates.Common.Infrastructure.Attributes;
using NoKates.Common.Infrastructure.Configuration;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace NoKates.Common.Infrastructure.Controllers
{
    [Route("NoKates/")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        public static Dictionary<string, List<string>> EndpointGroups;

        private readonly IEnumerable<EndpointDataSource> _endpointSources;

        public StatusController(
            IEnumerable<EndpointDataSource> endpointSources
        ) => _endpointSources = endpointSources;

        [HttpGet("ServiceStatus")]
        [EndpointGroup("NoKatesAdmin")]
        public ActionResult<ServiceStatus> GetStatus()
            => StatusValues.Status;

        [HttpGet("Log")]
        [EndpointGroup("NoKatesAdmin")]
        public ActionResult<List<LogEntry>> GetLog()
            => StatusValues.EventLog.ToList();

        [HttpGet("Requests")]
        [EndpointGroup("NoKatesAdmin")]
        public ActionResult<List<RequestMetric>> GetRequestResponses()
        => StatusValues.RequestMetrics?.Where(rm => !rm?.RequestPath?.ToLower()?.StartsWith("/nokates")??false).ToList() ?? new List<RequestMetric>();

        [HttpGet("EndpointGroups")]
        [EndpointGroup("NoKatesAdmin")]
        public ActionResult<Dictionary<string, List<string>>> GetEndpointGroups()
            => EndpointGroups;

        [HttpGet("Endpoints")]
        [EndpointGroup("NoKatesAdmin")]
        public ActionResult<List<EndpointDescription>> ListAllEndpoints()
            => AppHelper.GetEndpoints(_endpointSources.ToList());

    }
}
