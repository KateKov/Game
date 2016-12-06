using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities.Translation
{
    public class PlatformTypeTranslate : EntityTranslate, IEntityNamed
    {
        [Required]
        [StringLength(65)]
        [Index("IZ_type", 1, IsUnique = true)]
        public string Name { get; set; }
        public virtual PlatformType PlatformType { get; set; }
    }
}
