using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Roles
{
    public class RoleViewModel
    {
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ErrorName")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        public string Name { get; set; }

        public string Key { get; set; }

        public ICollection<string> Users { get; set; }

    }
}