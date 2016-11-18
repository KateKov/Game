using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class Genre : IEntityNamed
    {
        public Genre()
        {
            ChildGenres = new List<Genre>();
            Games =new List<Game>();
        }
        [Key]
        public Guid Id { get; set; }
        [DisplayName("Жанр")]
        [StringLength(65)]
        [Index("IY_name", 1, IsUnique = true)]
        [Required]
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public virtual Genre ParentGenre { get; set; }
        public virtual ICollection<Genre> ChildGenres { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}