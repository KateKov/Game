using GameStore.BLL.Interfaces;
using System;
using System.Collections.Generic;

namespace GameStore.BLL.DTO
{
    public class OrderDTO : IDtoBase
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public bool IsConfirmed { get; set; }
        public ICollection<OrderDetailDTO> OrderDetails { get; set; }
    }
}
