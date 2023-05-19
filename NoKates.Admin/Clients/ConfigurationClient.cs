using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;

namespace NoKates.Admin.Clients
{
    public interface IConfigurationClient
    {
        public string GetConfigWithoutDefaultsByApplicationName(string applicationName, string authToken);
        public string GetConfigWithDefaultsByApplicationName(string applicationName, string authToken);
        public string GetConfigRawByApplicationName(string applicationName, string authToken);
        public void SaveConfigByApplicationName(string applicationName, string config, string authToken);
        public List<string> GetFiles(string authToken);

    }
    public class ConfigurationClient: IConfigurationClient
    {
        private readonly string _baseUrl;

        public ConfigurationClient(string baseUrl)
        {
            this._baseUrl = baseUrl;
        }

        public string GetConfigWithoutDefaultsByApplicationName(string applicationName, string authToken)
            => JsonConvert.SerializeObject(HttpHelper.Get<JObject>($"{_baseUrl}/{applicationName}/NoDefaults", authToken).ThrowIfError(), Formatting.Indented);


        public string GetConfigWithDefaultsByApplicationName(string applicationName, string authToken)
            => JsonConvert.SerializeObject(HttpHelper.Get<JObject>($"{_baseUrl}/{applicationName}", authToken).ThrowIfError(), Formatting.Indented);


        public string GetConfigRawByApplicationName(string applicationName, string authToken)
            => JsonConvert.SerializeObject(HttpHelper.Get<JObject>($"{_baseUrl}/{applicationName}/raw", authToken).ThrowIfError(), Formatting.Indented);


        public void SaveConfigByApplicationName(string applicationName, string config, string authToken)
            => HttpHelper.Post($"{_baseUrl}/{applicationName}", config, authToken);


        public List<string> GetFiles(string authToken)
            => HttpHelper.Get<List<string>>($"{_baseUrl}/", authToken).ThrowIfError();


    }
}
