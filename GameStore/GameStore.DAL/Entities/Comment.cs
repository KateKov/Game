using GameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Entities
{
    public class Comment: IEntityBase
    {
        [Key]     
        public int Id { get; set; }
        [Required]
        [DisplayName("Имя")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [DisplayName("Тело комментария")]
        public string Body { get; set; }
        public virtual Game Game { get; set; }
        public virtual Comment ParentComment { get; set; }
        public virtual ICollection<Comment> ChildComments { get; set; }


        [ForeignKey("Game")]
        public int GameId { get; set; }
        public int ParentId { get; set; }

    }
}
