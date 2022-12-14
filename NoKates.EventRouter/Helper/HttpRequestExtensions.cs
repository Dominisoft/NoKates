using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace NoKates.EventRouter.Helper
{
    public static class HttpRequestExtensions
    {
        public static string GetRawBody(
            this HttpRequest request,
            Encoding encoding = null)
        {
            if (!request.Body.CanSeek)
            {
                // We only do this if the stream isn't *already* seekable,
                // as EnableBuffering will create a new stream instance
                // each time it's called
                request.EnableBuffering();
            }

            request.Body.Position = 0;

            var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);

            var body = reader.ReadToEnd();

            request.Body.Position = 0;

            return body;
        }
    }
}