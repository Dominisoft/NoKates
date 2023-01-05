using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NoKates.Common.Infrastructure.Attributes;
using NoKates.Configuration.Application;

namespace NoKates.Configuration.Controllers
{
    [Route("")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [HttpGet("{applicationName}/NoDefaults")]
        [EndpointGroup("Configuration:Read")]
        [NoAuth]
        public string GetConfigWithoutDefaultsByApplicationName(string applicationName)
        => _configurationService.GetConfigWithoutDefaults(applicationName);


        [HttpGet("{applicationName}")]
        [EndpointGroup("Configuration:Read")]
        [NoAuth]
        public string GetConfigWithDefaultsByApplicationName(string applicationName)
            => _configurationService.GetConfigWithDefaults(applicationName);

        [HttpGet("{applicationName}/raw")]
        [EndpointGroup("Configuration:Read")]
        [NoAuth]
        public string GetConfigRawByApplicationName(string applicationName)
            => _configurationService.GetConfigRaw(applicationName);

        [HttpPost("{applicationName}")]
        [EndpointGroup("Configuration:Write")]
        public void SaveConfigByApplicationName(string applicationName, [FromBody] configValue value )
            => _configurationService.SaveConfig(applicationName, value.innerString);
        [HttpGet]
        [EndpointGroup("Configuration:Admin")]
        public List<string> GetFiles()
            => _configurationService.ListFiles();


 

    }

    public class configValue
    {
        public string innerString { get; set; }
    }
}
