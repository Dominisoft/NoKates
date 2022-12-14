using System.Threading.Tasks;
using NoKates.Configuration.Extensions;
using Microsoft.AspNetCore.Http;

namespace NoKates.Configuration.Middleware
{
    public class LocalOnlyMiddleware
    {
        private readonly RequestDelegate _next;

        public LocalOnlyMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {

            if (!context.Request.IsLocal())
            {
                // Forbidden http status code
                context.Response.StatusCode = 403;
                return;
            }

            await _next(context);

        }
    }
}
