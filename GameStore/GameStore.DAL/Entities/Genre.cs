using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class Genre : IEntityBase, IEntityNamed
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Жанр")]
        [StringLength(65)]
        [Index("IY_name", 1, IsUnique = true)]
        [Required]
        public string Name { get; set; }
        public int ParentId { get; set; }
        public virtual Genre ParentGenre { get; set; }
        public virtual ICollection<Genre> ChildGenres { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}