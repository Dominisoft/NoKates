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
    public class EditUserModel : PageModel
    {


        public User OriginalUser { get; set; }
        public List<Role> Roles { get; set; }
        public Dictionary<string, List<string>> EndpointGroups { get; set; }
        public EditUserModel()
        {
            EndpointGroups = new Dictionary<string, List<string>>();
            OriginalUser = new User();
            Roles = new List<Role>();
             
        }
        public void OnGet()
        {
            var token = Request.Cookies["AccessToken"];
            if (string.IsNullOrWhiteSpace(token))
            {
                HttpContext.RedirectToRelativePath("Login");
                return;
            }
            if (Request.Query.ContainsKey("UserId"))
            {
                var userId = Convert.ToInt32(Request.Query["UserId"]);
                var userClient = new UserClient(token)
                {
                    BaseUrl = GlobalConfig.UserEndpointUrl,
                };

                OriginalUser = userClient.Get(userId);
            }
            else
            {
                OriginalUser = new User();
            }
            var roleClient = new RoleClient(token)
            {
                BaseUrl = GlobalConfig.RoleEndpointUrl,
            };

            Roles = roleClient.GetAll();

            EndpointGroups = HttpHelper.Get<Dictionary<string, List<string>>>(GlobalConfig.EndpointGroupsUrl, token);

        }

        public string GetRoleCheckedStatus(int role)
        {
            var roles = OriginalUser?.Roles?.Deserialize<List<int>>()??new List<int>();
            return roles.Contains(role) ? "checked=\"checked\"" : "";
        }
        public string GetActiveCheckedStatus(bool isActive)
        {
            return isActive ? "checked=\"checked\"" : "";
        }
        public string GetCheckedStatus(string Endpoint)
        {
            var result = OriginalUser?.AdditionalEndpointPermissions?.Contains(Endpoint) ?? false;
            return result ? "checked=\"checked\"" : "";
        }
        public string GetGroupCheckedStatus(string group)
        {
            var endpoints = EndpointGroups[group];
            var result = endpoints.All(e => OriginalUser?.AdditionalEndpointPermissions?.Contains(e) ?? false);
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
            var roles = form
    .Keys
    .Where(e => e.StartsWith("Role "))
    .Select(e => int.Parse(e.Substring(5)))
    .ToList();
            int.TryParse(Request.Query["UserId"], out var UserId);
            var updatedUser = new User
            {
                Id = UserId,
                Username = form["Input.Name"],
                Email = form["Input.Email"],
                FirstName = form["Input.FirstName"],
                LastName = form["Input.LastName"],
                PhoneNumber = form["Input.PhoneNumber"],
                AdditionalEndpointPermissions = endpoints?.Distinct()?.Serialize() ?? "[]",
                Roles = roles?.Distinct()?.Serialize() ?? "[]",
                IsActive = form.ContainsKey("Input.IsActive")

            };
            var userClient = new UserClient(token)
            {
                BaseUrl = GlobalConfig.UserEndpointUrl,
            };

            if (updatedUser.Id > 0)
            {
                //update
                var id = userClient.Update(updatedUser).Id;
                HttpContext.RedirectToRelativePath("Users");
                return;
            }
            else
            {
                //Create
                var id = userClient.Create(updatedUser).Id;
                HttpContext.RedirectToRelativePath("Users");
                return;
            }
        }
    }
}
