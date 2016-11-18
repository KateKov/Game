using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.ViewModels
{
    public class VisaViewModel
    {
        [Required(ErrorMessage = "Enter the Name!")]
        [RegularExpression("^[\\w]{1,50}", ErrorMessage = "The name isn't correct")]
        [DisplayName("Cart holder's name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter the Card Number!")]
        [DisplayName("Card number")]
        [RegularExpression("^[\\d]{16}", ErrorMessage = "The length must be 16 charachter")]
        public string Number { get; set; }
        [Required]
        [Range(1,12, ErrorMessage = "Month can be from 1 to 12")]
        [DisplayName("Month")]
        public int Month { get; set; }
        [Required]
        [Range(2016, 2100, ErrorMessage = "Year can be from 2016 to 2100")]
        [DisplayName("Year")]
        public int Year { get; set; }
        [Required]
        [RegularExpression("^[\\d]{3}", ErrorMessage = "The cvv2 number isn't correct")]
        public string CVV2 { get; set; }
        public OrderViewModel Order { get; set; }
    }
}