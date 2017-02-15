using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Genres
{
    public class GenreViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ErrorName")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "ParentGenre")]
        public string ParentName { get; set; }

        public string Key { get; set; }
    }
}