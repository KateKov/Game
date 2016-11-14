using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.ViewModels
{
    public class CommentViewModel
    {
        [Required(ErrorMessage = "Comment doesn't have ID")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Comment doesn't have Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Comment doesn't have Body")]
        [Display(Name = "Body")]
        public string Body { get; set; }
        [Required(ErrorMessage = "Comment doesn't have Game")]
        [Display(Name = "Game")]
        public string GameId { get; set; }
        public string GameKey { get; set; }
        [Display(Name = "Parent Name")]
        public string ParentCommentName { get; set; }
        [Display(Name = "Parent")]
        public string ParentCommentId { get; set; }
        public virtual ICollection<CommentViewModel> ChildComments { get; set; }
        public string Quote { get; set; }
        public bool IsDeleted { get; set; }
    }
}
