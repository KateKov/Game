using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.ViewModels
{
    public class CommentViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string GameId { get; set; }
        public string GameKey { get; set; }
        public CommentViewModel ParentComment { get; set; }
    }
}