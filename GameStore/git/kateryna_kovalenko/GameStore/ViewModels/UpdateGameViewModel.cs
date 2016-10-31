using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.ViewModels
{
    public class UpdateGameViewModel
    {
        public GameViewModel game;
        public List<GenreViewModel> genres;
        public List<PlatformTypeViewModel> types;
    }
}