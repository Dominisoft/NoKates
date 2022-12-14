using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace NoKates.EventRouter.Helper
{
    public static class TransformHelper
    {
        public static string ReplaceValues(string template, string values)
        {

            var result = template;

            Regex expression = new Regex(@"{{(?<Identifier>[A-Za-z0-9.\[\]]*)}}");
            var results = expression.Matches(result);
            foreach (Match match in results)
            {
                var name = match.Groups["Identifier"].Value;
                var j = JObject.Parse(values);
                var token = j.SelectToken(name);
                result = result.Replace($"{{{{{name}}}}}", token.ToString());
            }


            return result;


        }
    }
}
