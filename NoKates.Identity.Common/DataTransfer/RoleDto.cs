using NoKates.Common.Models;

namespace NoKates.Identity.Common.DataTransfer
{
    public class RoleDto:Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AllowedEndpoints { get; set; }
    }
}
