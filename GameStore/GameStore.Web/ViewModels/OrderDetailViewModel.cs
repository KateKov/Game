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
        public string Id { get; set; }
        [Display(Name = "Name")]
        public decimal Price { get; set; }
        [Display(Name = "Quantity")]
        public short Quantity { get; set; }
        [Display(Name = "Discount")]
        public float Discount { get; set; }
        public string OrderId { get; set; }
        [Required]
        public string GameId { get; set; }
        public string GameKey { get; set; }
    }
}