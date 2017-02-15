using GameStore.Web.App_LocalResources;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.ViewModels
{
    public class GenreViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ErrorName")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        public string Name { get; set; }

        public string ParentId { get; set; }

        public string ParentName { get; set; }
    }
}