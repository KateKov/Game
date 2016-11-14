using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.ViewModels
{
    public class CreatePublisherViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "HomePage")]
        public string HomePage { get; set; }
        public List<GameViewModel> Games { get; set; }
        public List<string> SelectedGames { get; set; }
    }
}