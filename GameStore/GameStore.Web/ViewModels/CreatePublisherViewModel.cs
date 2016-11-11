using System.Collections.Generic;

namespace GameStore.Web.ViewModels
{
    public class CreatePublisherViewModel
    {
        public PublisherViewModel Publisher { get; set; }
        public List<GameViewModel> Games { get; set; }
        public List<string> SelectedGames { get; set; }
    }
}