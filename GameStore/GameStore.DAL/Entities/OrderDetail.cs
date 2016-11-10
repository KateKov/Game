using System;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class OrderDetail: IEntityBase
    {
        public int Id { get; set; }
        public Decimal Price { get; set; }
        public short Quality { get; set; }
        public float Discount { get; set; }
        public virtual Order Order { get; set; }
        public virtual Game Game { get; set; }
    }
}
