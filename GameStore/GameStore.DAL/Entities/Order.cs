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
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
