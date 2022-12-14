using Microsoft.AspNetCore.Mvc;
using NoKates.Common.Infrastructure.Controllers;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Identity.Models;

namespace NoKates.Identity.Controllers
{
    [Route("[controller]")]
    public class RoleController : BaseController<Role>
    {
        public RoleController() : base(RepositoryHelper.CreateRepository<Role>())
        { }

    }
}
