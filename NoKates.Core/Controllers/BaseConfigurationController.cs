using Microsoft.AspNetCore.Mvc;
using NoKates.Common.Infrastructure.Attributes;

namespace NoKates.Core.Controllers
{
    [Route("Startup")]
    [ApiController]
    public class BaseConfigurationController : ControllerBase
    {
        [HttpGet("")]
        [EndpointGroup("Configuration:read")]
        [NoAuth]

        public ActionResult<string> GetStartupConfig()
        {
            var config = System.IO.File.ReadAllText(GlobalConfig.StartupConfig);
            return config;
        }
       

    }
}