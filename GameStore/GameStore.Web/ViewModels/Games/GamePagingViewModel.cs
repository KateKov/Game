using System.Collections.Generic;
using GameStore.DAL.Enums;

namespace GameStore.Web.ViewModels.Games
{
    public class GamePagingViewModel
    {
        public IEnumerable<GameViewModel> Games { get; set; }
        public int Page { get; set; }
        public PageEnum PageSize { get; set; }
        public int TotalItemsCount { get; set; }
    }
}