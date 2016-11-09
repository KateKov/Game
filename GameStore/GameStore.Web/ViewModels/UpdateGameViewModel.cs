using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GameStore.Web.ViewModels
{
    public class UpdateGameViewModel
    {
        [Required]
        public GameViewModel Game { get; set; }
        public List<GenreViewModel> Genres { get; set; }
        public List<PlatformTypeViewModel> Types { get; set; }
        public List<PublisherViewModel> Publishers { get; set; } 
    }
}