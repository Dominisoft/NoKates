using System.Collections.Generic;

namespace NoKates.Identity.Models
{
    public class UserRoles
    {
        public User User { get; set; }
        public List<Role> Roles { get; set; }
    }
}
