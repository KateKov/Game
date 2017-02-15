using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Orders
{
    public class OrderViewModel
    {
        [Required]
        public string Id { get; set; }

        public string CustomerId { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Sum")]
        public decimal Sum { get; set; }

        public bool IsConfirmed { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "OrderDetails")]
        public ICollection<OrderDetailViewModel> OrderDetails { get; set; }

        public ICollection<string> OrderDetailsId { get; set; }

        public DateTime? ShippedDate { get; set; }

        public bool IsShipped { get; set; }
    }
}