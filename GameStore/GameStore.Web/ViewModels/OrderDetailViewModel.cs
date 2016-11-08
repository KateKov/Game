using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Web.ViewModels
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public short Quality { get; set; }
        public float Discount { get; set; }
    }
}