using System;

namespace NoKates.Common.Infrastructure.CustomExceptions
{
    public class RequestException:Exception
    {
        public int StatusCode { get;private set; }

        public RequestException(int statusCode, string message):base(message)
            => StatusCode = statusCode;

    }
}
