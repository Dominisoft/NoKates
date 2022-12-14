using Microsoft.AspNetCore.Mvc;
using NoKates.Common.Infrastructure.Controllers;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.Identity.Controllers
{
    [Route("[controller]")]

    public class UserController : BaseController<User>
    {
        public UserController() : base(RepositoryHelper.CreateRepository<User>())
        { }

    }
}
