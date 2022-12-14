using System.Collections.Generic;
using NoKates.Common.Models;

namespace NoKates.Identity.Models
{
    public class UserRoles
    {
        public User User { get; set; }
        public List<Role> Roles { get; set; }
    }
}
