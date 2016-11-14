using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.ViewModels
{
    public class PlatformTypeViewModel
    {
        [Required(ErrorMessage = "PlatformType doesn't have Id")]
        public string Id { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "PlatformType doesn't have Name")]
        public string Name{ get; set; }
    }
}