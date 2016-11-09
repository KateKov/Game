using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameStore.Web.ViewModels
{
    public class OrderViewModel
    {
        [Required]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        public ICollection<OrderDetailViewModel> OrderDetails { get; set; }
    }
}