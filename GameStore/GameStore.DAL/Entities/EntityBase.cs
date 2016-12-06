using System;
using System.ComponentModel.DataAnnotations;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.MongoEntities
{
    public class EntityBase : IEntityBase
    {
        [Key]
        public Guid EntityId { get; set; }

        public string Id { get; set; }
    }
}
