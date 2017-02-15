using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class CommentDTO : IDtoNamed
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string GameId { get; set; }
        public string GameKey { get; set; }
        public string ParentCommentId { get; set; }
        public string ParrentCommentName { get; set; }
        public string Quote { get; set; }
    }
}
