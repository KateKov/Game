using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels
{
    public class CreatePublisherViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ErrorName")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "HomePage")]
        public string HomePage { get; set; }

        public List<GameViewModel> Games { get; set; }

        public List<string> SelectedGames { get; set; }
    }
}