using System.Collections.Generic;
using System.Linq;
using NoKates.WebConsole.Clients;
using NoKates.WebConsole.Extensions;
using NoKates.WebConsole.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NoKates.WebConsole.Pages.ServiceManagement
{
    public class AddOrRemoveAppsModel : PageModel
    {
        private IWebHostManagementClient _webHostManagementClient;

        public AddOrRemoveAppsModel(IWebHostManagementClient webHostManagementClient)
        {
            _webHostManagementClient = webHostManagementClient;
        }


        public List<string> UnreferencedDirectories { get; set; } = new List<string>();
        public List<string> Apps { get; set; } = new List<string>();
        public string AddApp { get; set; }
        public string RemoveApp { get; set; }

        public  void OnGet()
        {
            var token = Request.GetToken();
            if (TokenHelper.TokenIsInvalid(token))
            {
                HttpContext.Response.Redirect("Session/Login");
                return;
            }
            RemoveApp = Request.Query[nameof(RemoveApp)];
            AddApp = Request.Query[nameof(AddApp)];
            if (string.IsNullOrWhiteSpace(RemoveApp) && string.IsNullOrWhiteSpace(AddApp))
            {
                LoadData(token);
                return;
            }

            if (!string.IsNullOrWhiteSpace(AddApp))
                Add();
            else
                Remove();
        }

        private void Remove()
        {
            var token = Request.GetToken();
            var r1 = _webHostManagementClient.RemoveApp(RemoveApp, token);
            var r2=_webHostManagementClient.RemoveAppPool(RemoveApp, token);
            if (r1&&r2)Response.Redirect("./StatusList");

        }

        private void Add()
        {
            var token = Request.GetToken();
            var r1 = _webHostManagementClient.AddAppPool(AddApp, token);
            var r2 = _webHostManagementClient.AddApp(AddApp, token);
            if (r1 && r2) Response.Redirect("./StatusList");
        }

        public  void LoadData(string token)
        {
            UnreferencedDirectories = _webHostManagementClient.GetUnreferencedDirectories(token);
            Apps = _webHostManagementClient.GetAppPools(token).Keys.ToList();
           
        }

       
    }
}
