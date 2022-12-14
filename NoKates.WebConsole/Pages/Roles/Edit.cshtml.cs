using System;
using System.Collections.Generic;
using System.Linq;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Identity.Common.Clients;
using NoKates.WebConsole.Clients;
using NoKates.WebConsole.Extensions;
using NoKates.WebConsole.Helpers;
using NoKates.WebConsole.Pages.Shared;
using NoKates.Identity.Common.DataTransfer;

namespace NoKates.WebConsole.Pages.Roles
{
    public class EditModel : AuthenticatedPage
    {
        private readonly IRoleClient _roleClient;
        private readonly INoKatesCoreClient _adminClient;
        public RoleDto Role { get; set; }
        public Dictionary<string, List<string>> EndpointGroups { get; set; }
        public EditModel(IRoleClient roleClient, INoKatesCoreClient adminClient)
        {
            _roleClient = roleClient;
            _adminClient = adminClient;
            EndpointGroups = new Dictionary<string, List<string>>();
            Role = new RoleDto();
        }
        public override void LoadData(string token)
        {

            if (Request.Query.ContainsKey("RoleId"))
            {
                var roleId = Convert.ToInt32(Request.Query["RoleId"]);
                Role = _roleClient.Get(roleId).ThrowIfError();
            }
            else
            {
                Role = new RoleDto();
            }

            EndpointGroups = _adminClient.GetEndpointGroups(token);
        }
        public string GetCheckedStatus(string endpoint)
        {
            var result = Role?.AllowedEndpoints?.Contains(endpoint) ?? false;
            return result ? "checked=\"checked\"" : "";
        }
        public string GetGroupCheckedStatus(string group)
        {
            var endpoints = EndpointGroups[group];
            var result = endpoints.All(e => Role?.AllowedEndpoints?.Contains(e) ?? false);
            return result ? "checked=\"checked\"" : "";
        }

        public void OnPost()
        {

            var form = Request.Form;
            var endpoints = form
                .Keys
                .Where(e => e.StartsWith("EndpointPermission "))
                .Select(e => e.Substring(19))
                .ToList();
            int.TryParse(Request.Query["RoleId"], out var roleId);
            var updatedRole = new RoleDto
            {
                Id = roleId,
                Name = form["Input.Name"],
                Description = form["Input.Description"],
                AllowedEndpoints = endpoints?.Serialize()??"[]"
            };

            var token = Request.GetToken();

            HttpHelper.SetToken(token);

            if (updatedRole.Id > 0)
                _roleClient.Update(updatedRole);
            else
                _roleClient.Create(updatedRole);
            
            HttpContext.RedirectToRelativePath("Roles/list");

        }
    }
}
