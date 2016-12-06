using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.DAL.Interfaces;
using GameStore.DAL.MongoEntities;

namespace GameStore.DAL.Entities
{
    public class Order: EntityBase
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
        public string CustomerId { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal Sum { get; set; }

        public bool IsPayed { get; set; }
    }

}
