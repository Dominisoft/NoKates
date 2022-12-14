using Microsoft.AspNetCore.Mvc;
using NoKates.Common.Infrastructure.Attributes;
using NoKates.Common.Infrastructure.Configuration;
using file = System.IO.File;

namespace NoKates.Configuration.Controllers
{
    [Route("")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        [HttpGet("{applicationName}")]
        [EndpointGroup("ConfigurationRead")]
        [NoAuth]
        public string GetConfigByApplicationName(string applicationName)
        {
            ConfigurationValues.TryGetValue(out var configDir, "ConfigurationDirectory");
            var filePath = $"{configDir}/{applicationName}.json";
            if (file.Exists(filePath))
            {
                var template = file.ReadAllText(filePath);
                return ConfigurationTransformHelper.ReplaceStaticValues(template); 
            }
            return "{}";
        }
    }
}
