using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class Order: IEntityBase
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal Sum { get; set; }
    }

}
