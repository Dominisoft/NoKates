using NoKates.Common.Infrastructure.CustomExceptions;
using NoKates.Common.Models;

namespace NoKates.Common.Infrastructure.Extensions
{
    public static class RestResponseExtensions
    {
        public static TObject ThrowIfError<TObject>(this RestResponse<TObject> response) where TObject : class
        {
            if (response.IsError)
                throw new BadResponseException(response.Message);
            return response.Object;
        }
    }
}
