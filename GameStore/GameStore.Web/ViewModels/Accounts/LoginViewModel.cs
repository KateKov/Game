using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Accounts
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
         ErrorMessageResourceName = "RequiredError")]
        [StringLength(50, ErrorMessageResourceType = typeof(GlobalRes),
         ErrorMessageResourceName = "NameRangeError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "UserName")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
           ErrorMessageResourceName = "RequiredError")]
        [StringLength(50)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(GlobalRes), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}