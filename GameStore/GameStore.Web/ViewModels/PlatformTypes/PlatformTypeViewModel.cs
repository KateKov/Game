using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.PlatformTypes
{
    public class PlatformTypeViewModel
    {
        [Required]
        public string Id { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ErrorName")]
        public string Name{ get; set; }

        public string Key { get; set; }
    }
}