using System.Collections.Generic;
using GameStore.DAL.Enums;

namespace GameStore.Web.ViewModels
{
    public class GameFilteringViewModel
    {
        public IEnumerable<GameViewModel> Games { get; set; }
        public FilterViewModel Filter { get; set; }
        public int Page { get; set; }
        public PageEnum PageSize { get; set; }
        public int TotalItemsCount { get; set; }
    }
}