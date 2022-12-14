using NoKates.Common.Infrastructure.Configuration;
using NUnit.Framework;

namespace NoKates.Common.Tests.Infrastructure.Configuration
{
    [TestFixture]
    public class ConfigurationValuesTests
    {
        private const string EchoUrl = "https://httpbin.org/anything";

        [Test]
        public void ConfigurationValueShouldLoadFromUrl()
        {
            ConfigurationValues.LoadConfigFromUrl("http://DevAppServer/Configuration/Hello");
            ConfigurationValues.TryGetValue(out var method, "Name");
            Assert.AreEqual("ale",method);
        }


    }
}
