using System.Collections.Generic;

namespace GameStore.Web.ViewModels
{
    public class RenderCommentsMenuItem
    {
        public IEnumerable<CommentViewModel> MenuList { get; set; }
        public CommentViewModel Current { get; set; }
    }
}