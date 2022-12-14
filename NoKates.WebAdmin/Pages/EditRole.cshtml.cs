using System;
using System.Collections.Generic;
using System.Linq;
using NoKates.WebAdmin.Clients;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.WebAdmin.Pages
{
    public class EditRoleModel : PageModel
    {
        public Role Role { get; set; }
        public Dictionary<string, List<string>> EndpointGroups { get; set; }
        public EditRoleModel()
        {
            EndpointGroups = new Dictionary<string,List<string>>();
            Role = new Role();
        }
        public void OnGet()
        {
            var token = Request.Cookies["AccessToken"];
            if (string.IsNullOrWhiteSpace(token))
            {
                HttpContext.RedirectToRelativePath("Login");
                return;
            }
            if (Request.Query.ContainsKey("RoleId"))
            {
                var roleId = Convert.ToInt32(Request.Query["RoleId"]);
                var roleClient = new RoleClient(token)
                {
                   // BaseUrl = GlobalConfig.IdentityServiceUrl
                };
                
                Role = qroleClient.Get(roleId);
            }
            else
            {
                Role = new Role();
            }
            EndpointGroups = HttpHelper.Get<Dictionary<string, List<string>>>(GlobalConfig.EndpointGroupsUrl, token);

        }
        public string GetCheckedStatus(string Endpoint)
        {
            var result = Role?.AllowedEndpoints?.Contains(Endpoint) ?? false;
            return result ? "checked=\"checked\"" : "";
        }
        public string GetGroupCheckedStatus(string group)
        {
            var endpoints = EndpointGroups[group];
            var result = endpoints.All(e => Role?.AllowedEndpoints?.Contains(e)?? false) ;
            return result ? "checked=\"checked\"" : "";
        }

        public void OnPost()
        {
            var token = Request.Cookies["AccessToken"];

            var form = Request.Form;
            var endpoints = form
                .Keys
                .Where(e => e.StartsWith("EndpointPermission "))
                .Select(e => e.Substring(19))
                .ToList();
            int.TryParse(Request.Query["RoleId"], out var roleId);
            var updatedRole = new Role
            {
                Id = roleId,
                Name = form["Input.Name"],
                Description = form["Input.Description"],
                AllowedEndpoints = endpoints.Serialize()
            };

            var roleClient = new RoleClient(token)
            {
                BaseUrl = GlobalConfig.RoleEndpointUrl,
            };

            if (updatedRole.Id > 0)
            {
                //update
                var id = roleClient.Update(updatedRole).Id;
                HttpContext.RedirectToRelativePath("Roles");
                return;

            }
            else
            {
                //Create
                var id = roleClient.Create(updatedRole).Id;
                HttpContext.RedirectToRelativePath("Roles");
                return;

            }
        }
    }
}
