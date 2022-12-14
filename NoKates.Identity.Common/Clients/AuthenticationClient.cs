using System.Net;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.Identity.Common.Clients
{ 
    public interface IAuthenticationClient
    {
        RestResponse<string> Authenticate(string username, string password);
    }
    public class AuthenticationClient : IAuthenticationClient
{
    private readonly string _serviceRootUri;

    public AuthenticationClient(string serviceRootUri)
    {
        _serviceRootUri = serviceRootUri;
    }

    public RestResponse<string> Authenticate(string username,string password)
        => HttpHelper.Post<string>($"{_serviceRootUri}/Authentication",new NetworkCredential(username,password),string.Empty);


    }
}
