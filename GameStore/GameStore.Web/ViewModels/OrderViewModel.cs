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
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public decimal Sum { get; set; }
        public bool IsConfirmed { get; set; }
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        public ICollection<OrderDetailViewModel> OrderDetails { get; set; }
        public ICollection<string> OrderDetailsId { get; set; }
    }
}