using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.ViewModels
{
    public class GenreViewModel
    {
        [Required(ErrorMessage = "Genre doesn't have Id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Genre doesn't have Name")]
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public virtual ICollection<GenreViewModel> ChildGenres { get; set; }
    }
}