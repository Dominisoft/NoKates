using System;
using System.Collections.Generic;
using System.Linq;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;
using NoKates.Identity.Common.Clients;
using NoKates.WebConsole.Clients;
using NoKates.WebConsole.Extensions;
using NoKates.WebConsole.Helpers;
using NoKates.WebConsole.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoKates.Identity.Common.DataTransfer;

namespace NoKates.WebConsole.Pages.Users
{
    public class EditModel : AuthenticatedPage
    {
        private readonly IRoleClient _roleClient;
        private readonly INoKatesCoreClient _adminClient;
        private readonly IUserClient _userClient;
        public new UserDto User { get; set; }
        public List<RoleDto> Roles { get; set; }
        public Dictionary<string, List<string>> EndpointGroups { get; set; }


        public EditModel(IRoleClient roleClient, INoKatesCoreClient adminClient, IUserClient userClient)
        {
            _roleClient = roleClient;
            _adminClient = adminClient;
            _userClient = userClient;
            EndpointGroups = new Dictionary<string, List<string>>();
            User = new UserDto();
            Roles = new List<RoleDto>();

        }
        public override void LoadData(string token)
        {
            HttpHelper.SetToken(token);
            if (Request.Query.ContainsKey("UserId"))
            {
                var userId = Convert.ToInt32(Request.Query["UserId"]);


                User = _userClient.Get(userId).ThrowIfError();
            }
            else
            {
                User = new UserDto();
            }


            Roles = _roleClient.GetAll().ThrowIfError();

            EndpointGroups = _adminClient.GetEndpointGroups(token);

        }

        public string GetRoleCheckedStatus(int role)
        {
            var roles = User?.Roles?.Deserialize<List<int>>() ?? new List<int>();
            return roles.Contains(role) ? "checked=\"checked\"" : "";
        }
        public string GetActiveCheckedStatus(bool isActive)
        {
            return isActive ? "checked=\"checked\"" : "";
        }
        public string GetCheckedStatus(string Endpoint)
        {
            var result = User?.AdditionalEndpointPermissions?.Contains(Endpoint) ?? false;
            return result ? "checked=\"checked\"" : "";
        }
        public string GetGroupCheckedStatus(string group)
        {
            var endpoints = EndpointGroups[group];
            var result = endpoints.All(e => User?.AdditionalEndpointPermissions?.Contains(e) ?? false);
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
            int.TryParse(Request.Query["UserId"], out var userId);
            var updatedUser = new UserDto
            {
                Id = userId,
                Username = form["Input.Name"],
                Email = form["Input.Email"],
                FirstName = form["Input.FirstName"],
                LastName = form["Input.LastName"],
                PhoneNumber = form["Input.PhoneNumber"],
                AdditionalEndpointPermissions = endpoints?.Distinct()?.Serialize() ?? "[]",
                Roles = roles?.Distinct()?.Serialize() ?? "[]",
                IsActive = form.ContainsKey("Input.IsActive")

            };


            if (updatedUser.Id > 0)
                _userClient.Update(updatedUser);
            else
                _userClient.Create(updatedUser);
            HttpContext.RedirectToRelativePath("Users/List");

        }
    }
}
