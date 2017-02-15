using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{

    public class Comment : EntityBase, IEntityNamed
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        [Required]
        [ForeignKey("Game")]
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }
        public Guid? ParentCommentId { get; set; }
        public virtual Comment ParentComment { get; set; }
        public string Quote { get; set; }
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
