using System.Collections.Generic;

namespace GameStore.Web.ViewModels
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