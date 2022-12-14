using System.Collections.Generic;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Identity.Common.Clients;
using NoKates.WebConsole.Pages.Shared;
using NoKates.Identity.Common.DataTransfer;

namespace NoKates.WebConsole.Pages.Roles
{
    public class ListModel : AuthenticatedPage
    {
        private readonly IRoleClient _roleClient;

        public List<RoleDto> Roles { get; set; }

        public ListModel(IRoleClient roleClient)
        {
            _roleClient = roleClient;
        }



        public override void LoadData(string token)
        {
            HttpHelper.SetToken(token);

            Roles = _roleClient.GetAll().ThrowIfError();
        }
    }
}
