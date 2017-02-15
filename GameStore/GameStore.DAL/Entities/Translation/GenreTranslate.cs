using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities.Translation
{
    public class GenreTranslate : EntityTranslate, ITranslate
    {
        [DisplayName("Жанр")]
        [StringLength(65)]
        [Index("IY_name", 1, IsUnique = true)]
        public string Name { get; set; }
        public virtual Genre Genre { get; set; }
        [ForeignKey("Genre")]
        public Guid? BaseEntityId { get; set; }
    }
}
