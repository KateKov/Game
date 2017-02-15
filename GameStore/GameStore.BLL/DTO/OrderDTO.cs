using System;
using System.Collections.Generic;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class OrderDTO : IDtoBase
    {
        public string Id { get; set; }

        public string CustomerId { get; set; }

        public DateTime Date { get; set; }

        public decimal Sum { get; set; }

        public bool IsConfirmed { get; set; }

        public ICollection<OrderDetailDTO> OrderDetails { get; set; }

        public ICollection<string> OrderDetailsId { get; set; }

        public DateTime? ShippedDate { get; set; }

        public bool IsShipped { get; set; }
        public bool IsPayed { get; set; }
    }
}
