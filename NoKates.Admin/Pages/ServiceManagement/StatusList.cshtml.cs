using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoKates.Common.Infrastructure.Client;

namespace NoKates.Admin.Pages.ServiceManagement
{
    public class StatusListModel : PageModel
    {
        private readonly INoKatesCoreClient _client;


        public StatusListModel(INoKatesCoreClient client)
        {
            _client = client;
        }
        public List<string> Services;
        public void OnGet()
        {
            var token = Request.Cookies["AuthorizationToken"];
            Services = _client.GetAllServiceNames(token)?.Object?.ToList()??new List<string>();
        }
    }
}
