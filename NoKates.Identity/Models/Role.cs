using System.ComponentModel.DataAnnotations.Schema;
using NoKates.Common.Infrastructure.Attributes;
using NoKates.Common.Models;

namespace NoKates.Identity.Models
{
    [DefaultConnectionString("Identity")]
    [Table("Roles")]

    public class Role: Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AllowedEndpoints { get; set; }
    }
}
