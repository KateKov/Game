using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.DAL.Entities
{
    public class Order: EntityBase
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public DateTime? ShippedDate { get; set; }

        public bool IsShipped { get; set; }

        public bool IsConfirmed { get; set; }


        public decimal Sum { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsPayed { get; set; }
    }

}
