using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Web.ViewModels
{
    public class RenderCommentsMenuItem
    {
        public IEnumerable<CommentViewModel> MenuList { get; set; }
        public CommentViewModel Current { get; set; }
    }
}