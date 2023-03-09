using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace NoKates.Configuration
{
    public static class ConfigurationTransformHelper
    {


        public static string ReplaceStaticValues(string configTemplate, JObject masterValues)
        {
            var result = configTemplate;

            Regex expression = new Regex(@"{{(?<Identifier>[A-Za-z0-9.\[\]]*)}}");
            var results = expression.Matches(result);
            foreach (Match match in results)
            {
                var name = match.Groups["Identifier"].Value;
                var token = masterValues.SelectToken(name);
                result = result.Replace($"{{{{{name}}}}}", token?.ToString());
            }
            
            return result;

        }
    }
}
