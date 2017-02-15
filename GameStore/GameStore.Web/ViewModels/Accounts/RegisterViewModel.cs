using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Accounts
{
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
          ErrorMessageResourceName = "RequiredError")]
        [StringLength(50)]
        [Display(ResourceType = typeof(GlobalRes), Name = "UserName")]
        public string Username { get; set; }


        [Required(ErrorMessageResourceType = typeof(GlobalRes),
           ErrorMessageResourceName = "RequiredError")]
        [StringLength(50)]
        [Display(ResourceType = typeof(GlobalRes), Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "EmailError")]
        public string Email { get; set; }


        [Required(ErrorMessageResourceType = typeof(GlobalRes),
           ErrorMessageResourceName = "RequiredError")]
        [StringLength(50)]
        [Display(ResourceType = typeof(GlobalRes), Name = "Password")]
        public string Password { get; set; }


        [Required(ErrorMessageResourceType = typeof(GlobalRes),
           ErrorMessageResourceName = "RequiredError")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(GlobalRes), Name = "ConfirmePassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(GlobalRes),
           ErrorMessageResourceName = "ConfirmePasswordError")]
        public string ConfirmPassword { get; set; }
    }
}