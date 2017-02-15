using System.ComponentModel.DataAnnotations;
using GameStore.DAL.Enums;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Games
{
    public class GameTranslateViewModel
    {
        public string Id { get; set; }
        public Language Language { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Description")]
        [MaxLength(1000, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "DescriptionRangeError")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        [MaxLength(50, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "NameRangeError")]
        public string Name { get; set; }
    }
}