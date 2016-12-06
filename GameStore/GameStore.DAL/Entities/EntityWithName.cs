using System.ComponentModel.DataAnnotations;
using GameStore.DAL.Interfaces;
using GameStore.DAL.MongoEntities;

namespace GameStore.DAL.Entities
{
    public class EntityWithName : EntityBase, IEntityNamed
    {
        [Required]
        public string Name { get; set; }
    }
}
