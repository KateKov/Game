using System;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class ManagerProfile : IEntityBase
    {
        [ForeignKey("User")]
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }
        public virtual User User { get; set; }

        public NotificationMethod Method { get; set; }
    }
}
