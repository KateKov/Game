using System.ComponentModel.DataAnnotations;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class EntityWithName : EntityBase, IEntityNamed
    {
        [Required]
        public string Name { get; set; }
    }
}
