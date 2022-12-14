using System;
using System.Collections.Generic;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;
using NoKates.Identity.Common.Clients;
using NoKates.WebConsole.Extensions;
using NoKates.WebConsole.Helpers;
using NoKates.WebConsole.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoKates.Identity.Common.DataTransfer;

namespace NoKates.WebConsole.Pages.Users
{
    public class ListModel : AuthenticatedPage
    {
        private readonly IUserClient _userClient;

        public List<UserDto> Users { get; set; }

        public ListModel(IUserClient userClient)
        {
            _userClient = userClient;
        }


        public override void LoadData(string token)
        {
            HttpHelper.SetToken(token);

            Users = _userClient.GetAll().ThrowIfError();
        }
    }
}
