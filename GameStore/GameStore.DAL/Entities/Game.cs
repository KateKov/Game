using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Interfaces;
using GameStore.DAL.MongoEntities;

namespace GameStore.DAL.Entities
{
    public class Game : EntityBase, IEntityWithKey, ITranslateNamed<GameTranslate>
    {
        [Required]
        [StringLength(65)]
        [Index("IX_key", 1, IsUnique = true)]
        public string Key { get; set; }

        [Column(TypeName = "SMALLINT")]
        public short UnitsInStock { get; set; }

        public bool Discountinues { get; set; }

        [ForeignKey("Publisher")]
        public Guid? PublisherId { get; set; }

        public int Viewing { get; set; }
        public decimal Price { get; set; }

        public DateTime DateOfAdding { get; set; }

        public virtual Publisher Publisher { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }

        public virtual ICollection<PlatformType> PlatformTypes { get; set; }
        
        public virtual ICollection<GameTranslate> Translates { get; set; }
    }
}
