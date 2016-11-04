using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.ViewModels
{
    public class GameViewModel
    {
        [Required(ErrorMessage = "Game doesn't have Id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Game doesn't have Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<CommentViewModel> Comments { get; set; }
        [Required(ErrorMessage = "Game doesn't have genres")]
        public virtual ICollection<GenreViewModel> Genres { get; set; }
        [Required(ErrorMessage = "Game doesn't have platform type")]
        public virtual ICollection<PlatformTypeViewModel> PlatformTypes { get; set; }
        [Required(ErrorMessage = "Game doesn't have Key")]
        public string Key { get; set; }
    }
}
