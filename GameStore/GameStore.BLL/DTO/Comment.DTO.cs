using System.Collections.Generic;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class CommentDTO : IDtoBase, IDtoNamed
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string GameId { get; set; }
        public string GameKey { get; set; }
        public string ParentId { get; set; }
        public string ParrentComment { get; set; }
    }
}
