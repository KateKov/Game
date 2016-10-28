using GameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Entities
{
    public class Comment: IEntityBase
    {
        [Required]       
        public int Id { get; set; }
        [DisplayName("Имя")]
        public string Name { get; set; }
        [DisplayName("Тело комментария")]
        public string Body { get; set; }
        public virtual Game Game { get; set; }
        public int GameId { get; set; }
        public int ParentId { get; set; }

    }
}
