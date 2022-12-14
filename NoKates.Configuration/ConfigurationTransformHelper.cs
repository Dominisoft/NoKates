using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace NoKates.Configuration
{
    public static class ConfigurationTransformHelper
    {
        private static string masterJsonValues;
        public static void LoadMasterConfig(string path)
        {
            masterJsonValues = File.ReadAllText(path);
        }
        public static string ReplaceStaticValues(string configTemplate)
        {

            var result = configTemplate;

            Regex expression = new Regex(@"{{(?<Identifier>[A-Za-z0-9.\[\]]*)}}");
            var results = expression.Matches(result);
            foreach (Match match in results)
            {
                var name = match.Groups["Identifier"].Value;
                var j = JObject.Parse(masterJsonValues);
                var token = j.SelectToken(name);
                result = result.Replace($"{{{{{name}}}}}", token.ToString());
            }


            return result;


        }
    }
}
