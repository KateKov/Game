using GameStore.Web.App_LocalResources;
using GameStore.Web.ViewModels.Translates;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GameStore.Web.ViewModels.Roles
{
    public class CreateRoleViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ErrorName")]
        [MaxLength(50, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "NameRangeError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        public string Name { get; set; }

        public IList<TranslateViewModel> Translates { get; set; }

    }
}