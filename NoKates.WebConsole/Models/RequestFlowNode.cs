using System.Collections.Generic;
using NoKates.Common.Models;

namespace NoKates.WebConsole.Models
{
    public class RequestFlowNode
    {
        public RequestMetric Request { get; set; }
        public List<RequestFlowNode> SubRequests { get; set; }
    }
}
