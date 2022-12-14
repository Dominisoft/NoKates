using System.ComponentModel.DataAnnotations;

namespace NoKates.Common.Models
{
    public class Entity
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
