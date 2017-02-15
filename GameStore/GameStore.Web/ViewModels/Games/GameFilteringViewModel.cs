using System.Collections.Generic;

namespace GameStore.Web.ViewModels.Games
{
    public class GameFilteringViewModel
    {
        public IEnumerable<GameViewModel> Games { get; set; }
        public FilterViewModel Filter { get; set; }
    }
}