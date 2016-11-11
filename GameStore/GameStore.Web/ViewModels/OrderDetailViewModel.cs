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
        [Display(Name = "Quantity")]
        public short Quantity { get; set; }
        [Display(Name = "Discount")]
        public float Discount { get; set; }
        public int OrderId { get; set; }
        [Required]
        public int GameId { get; set; }
        public string GameKey { get; set; }
    }
}