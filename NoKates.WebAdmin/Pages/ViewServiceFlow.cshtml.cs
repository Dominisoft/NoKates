using System.Collections.Generic;
using System.Linq;
using NoKates.WebAdmin.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.WebAdmin.Pages
{
    public class ViewServiceFlowModel : PageModel
    {
        public ServiceFlowNode ServiceFlow;

        public ViewServiceFlowModel()
        {
            ServiceFlow = new ServiceFlowNode();
        }

        public void OnGet()
        {
            var token = Request.Cookies["AccessToken"];
            if (string.IsNullOrWhiteSpace(token))
            {
                HttpContext.RedirectToRelativePath("Login");
                return;
            }
            var requestId = Request.Query["requestId"];

            GetRequestFlow(requestId,token);

        }

        private void GetRequestFlow(string requestId,string token)
        {
            var requestMetrics = HttpHelper.Get<List<RequestMetric>>($"{GlobalConfig.RequestMetricUrl}{requestId}", token);

            ServiceFlow = MapRequestFlow(requestMetrics);
        }


        private ServiceFlowNode MapRequestFlow(List<RequestMetric> metrics)
        {
            var root = metrics.FirstOrDefault(m => m.RequestSource == "External" || 
                                                   m.RequestStart == metrics.Min(m2 => m2.RequestStart));
            
            var result = new ServiceFlowNode
            {
                Request = root
                ,SubRequests = GetSubRequests(root?.ServiceName,metrics,0)
            };
            
            return result;

        }

        private List<ServiceFlowNode> GetSubRequests(string requestSource, List<RequestMetric> metrics,int level)
        {
            if (level > 8) 
                return new List<ServiceFlowNode>();

            var requests = metrics.Where(m => m.RequestSource == requestSource);
            if (!requests?.Any()??false) return new List<ServiceFlowNode>();

            var newList = metrics.Where(r => !requests.Contains(r)).ToList();

            return requests.Select(request => new ServiceFlowNode
            {
                Request = request,
                SubRequests = GetSubRequests(request.ServiceName, newList, level+1)
            }).ToList();

        }
    }
}
