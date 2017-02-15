using System;
using System.ComponentModel.DataAnnotations;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class EntityBase : IEntityBase
    {
        [Key]
        public Guid Id { get; set; }


        public bool IsDeleted { get; set; }
    }
}
