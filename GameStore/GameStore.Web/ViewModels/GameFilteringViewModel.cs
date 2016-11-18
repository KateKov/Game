using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Web.ViewModels
{
    public class GameFilteringViewModel
    {
        public IEnumerable<GameViewModel> Games { get; set; }
        public FilterViewModel Filter { get; set; }
    }
}