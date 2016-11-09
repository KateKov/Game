using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class Comment : IEntityBase, IEntityNamed
    {
        [Key]     
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        [Required]
        [ForeignKey("Game")]
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public virtual Comment ParentComment { get; set; }
        public virtual ICollection<Comment> ChildComments { get; set; }
    }
}
