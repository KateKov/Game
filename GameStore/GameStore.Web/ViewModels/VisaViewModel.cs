using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels
{
    public class VisaViewModel
    {
        [Required(ErrorMessage = "Enter the Name!")]
        [RegularExpression("^[\\w]{1,50}", ErrorMessage = "The name isn't correct")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter the Card Number!")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Number")]
        [RegularExpression("^[\\d]{16}", ErrorMessage = "The length must be 16 charachter")]
        public string Number { get; set; }
        [Required]
        [Range(1,12, ErrorMessage = "Month can be from 1 to 12")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Month")]
        public int Month { get; set; }
        [Required]
        [Range(2016, 2100, ErrorMessage = "Year can be from 2016 to 2100")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Year")]
        public int Year { get; set; }
        [Required]
        [RegularExpression("^[\\d]{3}", ErrorMessage = "The cvv2 number isn't correct")]
        public string CVV2 { get; set; }
        public OrderViewModel Order { get; set; }
    }
}