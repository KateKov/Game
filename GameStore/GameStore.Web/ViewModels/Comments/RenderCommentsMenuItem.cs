using System.Collections.Generic;

namespace GameStore.Web.ViewModels.Comments
{
    public class RenderCommentsMenuItem
    {
        public IEnumerable<CommentViewModel> MenuList { get; set; }
        public CommentViewModel Current { get; set; }
    }
}