using System.Collections.Generic;
using NoKates.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using NoKates.Common.Infrastructure.Attributes;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

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
