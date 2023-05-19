using System.Collections.Generic;
using System.Linq;
using NoKates.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using NoKates.Common.Infrastructure.Attributes;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;
using Microsoft.Web.Administration;

namespace NoKates.Core.Controllers
{
    [Route("")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet("")]
        [EndpointGroup("System.Admin")]
        public ActionResult<ServiceStatus[]> GetAllServiceStatuses()
        {
            var root = AppHelper.GetRootUri();
            var paths = ServiceStatusHelper.GetApplicationStatusPagePaths(root);
            var results = new List<ServiceStatus>();
            foreach (var path in paths)
            {
                var status = ServiceStatusHelper.GetStatus(path);
                results.Add(status);
            }

            return results.ToArray();
        }
        [HttpGet("Services")]
        [EndpointGroup("System.Admin")]
        public ActionResult<string[]> GetAllServiceNames()
        {
            var apps = AppHelper.GetApps().Where(app => app.ApplicationPoolName != AppHelper.GetAppName());
            return apps.Select(a => a.ApplicationPoolName).ToArray();

        }
        [HttpGet("Pool/{name}")]
        [EndpointGroup("System.Admin")]
        public PoolState GetPool(string name)
        {
            using ServerManager serverManager = new ServerManager();
            var pools = serverManager.ApplicationPools;
            var pool = pools.FirstOrDefault(p => p.Name == name);
            return new PoolState
            {
                State = pool.State
            };
        }


        [HttpGet("EndpointGroups")]
        [EndpointGroup("System.Admin")]
        public ActionResult<Dictionary<string, List<string>>> GetEndpointGroups()
        {
            return ServiceStatusHelper.GetGroups($"{Request.Scheme}://{Request.Host.Value}/");
        }

        [HttpGet("favicon.ico")]
        [EndpointGroup("Public")]
        [NoAuth]
        public FileContentResult GetFavIcon()
        {
             var path = GlobalConfig.IconPath;
             return new FileContentResult(System.IO.File.ReadAllBytes(path ?? "./favicon.ico"), "image/x-icon");
        }

    }
}
