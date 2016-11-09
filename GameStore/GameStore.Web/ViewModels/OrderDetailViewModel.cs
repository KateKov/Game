using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameStore.Web.ViewModels
{
    public class OrderDetailViewModel
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public decimal Price { get; set; }
        [Display(Name = "Quality")]
        public short Quality { get; set; }
        [Display(Name = "Discount")]
        public float Discount { get; set; }
    }
}