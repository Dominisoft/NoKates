using NoKates.Common.Infrastructure.CustomExceptions;
using NUnit.Framework;

namespace NoKates.Common.Tests.Infrastructure.CustomExceptions
{
    [TestFixture]
    public class RequestExceptionTests
    {
        [Test]
        public void BaseRequestExceptionShouldContainCorrectMessageAndResponseCode()
        {
            #region Arrange

            const string msg = "Testing";
            const int code = 123;
            #endregion

            #region Act

            var exception = new RequestException(code,msg);

            #endregion

            #region Assert

            Assert.AreEqual(code, exception.StatusCode,"Incorrect Response Code");
            Assert.AreEqual(msg, exception.Message,"Incorrect Response Code");

            #endregion

        }
    }
}
