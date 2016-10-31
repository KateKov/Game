using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public int GameId { get; set; }
        public string GameKey { get; set; }
        public string ParentCommentName { get; set; }
        public int? ParentCommentId { get; set; }
        public virtual ICollection<CommentViewModel> ChildComments { get; set; }
    }

}
