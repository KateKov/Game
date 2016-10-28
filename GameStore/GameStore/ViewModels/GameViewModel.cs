using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;

namespace GameStore.ViewModels
{
    public class GameViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<CommentViewModel> Comments { get; set; }
        public List<string> Genres { get; set; }
        public List<string> PlatformTypes { get; set; }
    }
}