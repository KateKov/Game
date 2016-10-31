using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.ViewModels
{
    public class GenreViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public virtual ICollection<GenreViewModel> ChildGenres { get; set; }


    }
}