using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NoKates.Common.Infrastructure.Configuration;
using NoKates.Common.Infrastructure.Extensions;

namespace NoKates.Configuration.Application
{
    public interface IConfigurationService
    {
        string GetConfigWithoutDefaults(string applicationName);
        string GetConfigWithDefaults(string applicationName);
        string GetConfigRaw(string applicationName);
        void SaveConfig(string applicationName, string config);
        List<string> ListFiles();
    }
    public class ConfigurationService: IConfigurationService
    {
        private string DefaultValuesTemplateName => ConfigurationValues.Values["DefaultValuesTemplate"];
        private string MasterValuesTemplateName => ConfigurationValues.Values["MasterValuesTemplate"];

        private static JObject _masterValues;
        private static JObject _defaultValues;

        private readonly IConfigReader _configReader;

        public ConfigurationService(IConfigReader configReader)
        {
            _configReader = configReader;
            LoadMasterConfig(MasterValuesTemplateName);
            LoadDefaultConfig(DefaultValuesTemplateName);
        }

        public string GetConfigWithoutDefaults(string applicationName)
        {
            var template = GetConfigRaw(applicationName);
            return ConfigurationTransformHelper.ReplaceStaticValues(template,_masterValues);
        }

        public string GetConfigWithDefaults(string applicationName)
        {
            var template = GetConfigRaw(applicationName);

            var values = JObject.Parse(template);

            foreach (var val in _defaultValues)
            {
                if (!values.ContainsKey(val.Key))
                    values.Add(val.Key,val.Value);
            }

            return ConfigurationTransformHelper.ReplaceStaticValues(values.Serialize(),_masterValues);

        }

        public string GetConfigRaw(string applicationName)
            => _configReader.GetConfigFile(applicationName);

        public void SaveConfig(string applicationName, string config)
        {
            _configReader.WriteValues(applicationName, config);
        }

        public List<string> ListFiles()
        {
            return _configReader.ListFiles();
        }

        private void LoadMasterConfig(string name)
        {
            _masterValues = JObject.Parse(_configReader.GetConfigFile(name));
        }

        private  void LoadDefaultConfig(string name)
        {
            _defaultValues = JObject.Parse(_configReader.GetConfigFile(name));
        }

    }
}
