using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.DAL.Entities
{
    public class OrderDetail: EntityBase
    {
        [Column(TypeName = "SMALLINT")]
        public short Quantity { get; set; }
        public float Discount { get; set; }
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
        [ForeignKey("Game")]
        public Guid GameId { get; set; }
        public bool IsPayed { get; set; }
        public virtual Game Game { get; set; }
        public decimal Price { get; set; }
    }
}
