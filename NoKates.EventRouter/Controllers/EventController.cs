using System.Threading;
using Microsoft.AspNetCore.Mvc;
using NoKates.Common.Infrastructure.Attributes;
using NoKates.Common.Infrastructure.Controllers;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.EventRouter.Helper;

namespace NoKates.EventRouter.Controllers
{
    [Route("")]
    [ApiController]
    public class EventController : NoKatesControllerBase
    {
        [HttpGet("{routingKey}")]
        [EndpointGroup("Events")]
        [NoAuth]
        public bool PublishEventByRoutingKey(string routingKey)
        {
            var requestId = Thread.CurrentThread.GetRequestId();
            var eventDetails = Request.GetRawBody();

            EventRouter.ProcessEvent(routingKey,eventDetails, requestId);
            return true;
        }
    }
}
