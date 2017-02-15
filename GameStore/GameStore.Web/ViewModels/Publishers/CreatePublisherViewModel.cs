using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;
using GameStore.Web.ViewModels.Translates;

namespace GameStore.Web.ViewModels.Publishers
{
    public class CreatePublisherViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ErrorName")]
        [MaxLength(50, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "NameRangeError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        public string Name { get; set; }

        [MaxLength(1000, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "DescriptionRangeError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Description")]
        public string Description { get; set; }

        public IList<TranslateViewModelDescription> Translates { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "HomePage")]
        [DataType(DataType.Url, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "UrlError")]
        [Required]
        public string HomePage { get; set; }
    }
}