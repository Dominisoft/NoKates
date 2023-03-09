using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NoKates.Configuration
{
    public interface IConfigReader
    {
        string GetConfigFile(string name);
        void WriteValues(string applicationName, string config);
        List<string> ListFiles();
    }
    public class ConfigReader: IConfigReader
    {
        private readonly string _configurationDirectory;
        public ConfigReader(string configurationDirectory)
        {
            this._configurationDirectory = configurationDirectory;
        }

        public string GetConfigFile(string name)
        {
            return File.ReadAllText($"{_configurationDirectory}\\{name}.json");
        }

        public void WriteValues(string name, string config)
        {
            File.WriteAllText($"{_configurationDirectory}\\{name}.json",config);
        }

        public List<string> ListFiles()
        {
            return Directory.GetFiles(_configurationDirectory, "*.json").Select(Path.GetFileNameWithoutExtension).ToList();
        }
    }
}
