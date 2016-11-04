using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.ViewModels
{
    public class CommentViewModel
    {
        [Required(ErrorMessage = "Comment doesn't have ID")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Comment doesn't have Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Comment doesn't have Body")]
        public string Body { get; set; }
        [Required(ErrorMessage = "Comment doesn't have Game")]
        public int GameId { get; set; }
        public string GameKey { get; set; }
        public string ParentCommentName { get; set; }
        public int? ParentCommentId { get; set; }
        public virtual ICollection<CommentViewModel> ChildComments { get; set; }
    }
}
