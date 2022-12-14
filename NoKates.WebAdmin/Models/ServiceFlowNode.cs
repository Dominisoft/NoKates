using System.Collections.Generic;
using NoKates.Common.Models;

namespace NoKates.WebAdmin.Models
{
    public class ServiceFlowNode
    {
        public RequestMetric Request { get; set; }
        public List<ServiceFlowNode> SubRequests { get; set; }
    }
}
