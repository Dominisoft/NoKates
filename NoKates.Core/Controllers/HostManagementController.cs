using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NoKates.Core.Application;
using NoKates.Core.Infrastructure;

namespace NoKates.Core.Controllers
{
    [Route("PoolManagement")]
    [ApiController]
    public class HostManagementController : ControllerBase
    {
        private readonly IWebHostManagementService _webHostManagementService;

        public HostManagementController(IWebHostManagementService webHostManagementService)
        {
            _webHostManagementService = webHostManagementService;
        }

        [HttpGet("AppPools")] public Dictionary<string,bool> GetAppPools() => _webHostManagementService.GetAppPools();

        [HttpGet("UnreferencedDirectories")]
        public List<string> GetUnreferencedDirectories() => _webHostManagementService.GetUnrefrencedDirectories();
        [HttpPost("Start/{appPoolName}")] public ActionResult StartAppPool(string appPoolName) => _webHostManagementService.StartAppPool(appPoolName).ToActionResult();
        [HttpPost("Stop/{appPoolName}")] public ActionResult StopAppPool(string appPoolName) => _webHostManagementService.StopAppPool(appPoolName).ToActionResult();
        [HttpPost("AddPool/{appPoolName}")] public ActionResult AddAppPool(string appPoolName) => _webHostManagementService.AddAppPool(appPoolName).ToActionResult();
        [HttpPost("RemovePool/{appPoolName}")] public ActionResult RemoveAppPool(string appPoolName) => _webHostManagementService.RemoveAppPool(appPoolName).ToActionResult();
        [HttpPost("AddApp/{appName}")] public ActionResult AddApp(string appName) => _webHostManagementService.AddApp(appName).ToActionResult();
        [HttpPost("RemoveApp/{appName}")] public ActionResult RemoveApp(string appName) => _webHostManagementService.RemoveApp(appName).ToActionResult();


    }
}
