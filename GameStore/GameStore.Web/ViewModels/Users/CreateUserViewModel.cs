using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;
using GameStore.Web.ViewModels.Roles;

namespace GameStore.Web.ViewModels.Users
{
    public class CreateUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
         ErrorMessageResourceName = "RequiredError")]
        [StringLength(50)]
        [Display(ResourceType = typeof(GlobalRes), Name = "UserName")]
        public string Username { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
          ErrorMessageResourceName = "RequiredError")]
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "EmailError")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "IsLocked")]
        public bool IsLocked { get; set; }

        public DateTime CreateDate { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "IsDeleted")]
        public bool IsDeleted { get; set; }


        [Required(ErrorMessageResourceType = typeof(GlobalRes),
          ErrorMessageResourceName = "RequiredError")]
        public virtual ICollection<string> SelectedRoles { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Roles")]
        public virtual ICollection<RoleViewModel> AllRoles { get; set; }


        [StringLength(50)]
        [Display(ResourceType = typeof(GlobalRes), Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(GlobalRes), Name = "ConfirmePassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(GlobalRes),
           ErrorMessageResourceName = "ConfirmePasswordError")]
        public string ConfirmPassword { get; set; }
    }
}