using System.Collections.Generic;

namespace GameStore.Web.ViewModels
{
    public class UpdateGameViewModel
    {
        public GameViewModel Game { get; set; }
        public List<GenreViewModel> Genres { get; set; }
        public List<PlatformTypeViewModel> Types { get; set; }
        public List<PublisherViewModel> Publishers { get; set; } 
    }
}