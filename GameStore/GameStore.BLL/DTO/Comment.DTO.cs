using System.Collections.Generic;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class CommentDTO : IDtoBase, IDtoNamed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public int GameId { get; set; }
        public string GameKey { get; set; }
        public int? ParentId { get; set; }
        public string ParrentComment { get; set; }
        public virtual ICollection<CommentDTO> ChildComments { get; set; }  
    }
}
