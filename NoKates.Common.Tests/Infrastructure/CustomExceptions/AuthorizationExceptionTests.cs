using NoKates.Common.Infrastructure.CustomExceptions;
using NUnit.Framework;

namespace NoKates.Common.Tests.Infrastructure.CustomExceptions
{
    [TestFixture]
    public class AuthorizationExceptionTests
    {
        [Test]
        public void ExceptionShouldContainCorrectMessageAndResponseCode()
        {
            #region Arrange

            const string msg = "Testing";

            #endregion

            #region Act

            var exception = new AuthorizationException(msg);

            #endregion

            #region Assert

            Assert.AreEqual(401, exception.StatusCode,"Incorrect Response Code");
            Assert.AreEqual(msg, exception.Message,"Incorrect Response Code");

            #endregion

        }
    }
}
