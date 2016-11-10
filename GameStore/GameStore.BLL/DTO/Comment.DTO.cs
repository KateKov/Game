using System.Collections.Generic;

namespace GameStore.BLL.DTO
{
    public class CommentDTO
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
