using System.Collections.Generic;

namespace GameStore.Web.ViewModels
{
    public class GameViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<CommentViewModel> Comments { get; set; }
        public virtual ICollection<GenreViewModel> Genres { get; set; }
        public virtual ICollection<PlatformTypeViewModel> PlatformTypes { get; set; }
        public string Key { get; set; }
    }
}
