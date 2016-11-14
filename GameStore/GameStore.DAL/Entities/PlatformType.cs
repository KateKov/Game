using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class PlatformType : IEntityBase, IEntityNamed
    {
        public PlatformType()
        {
            Games = new List<Game>();
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(65)]
        [Index("IZ_type", 1, IsUnique = true)]
        public string Name { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}
