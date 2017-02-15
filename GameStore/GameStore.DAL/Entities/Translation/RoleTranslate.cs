using System;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities.Translation
{
    public class RoleTranslate : EntityTranslate, ITranslate
    {
        public string Name { get; set; }
        public virtual Role Role { get; set; }
        [ForeignKey("Role")]
        public Guid? BaseEntityId { get; set; }
    }
}
