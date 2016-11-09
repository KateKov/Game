using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GameStore.DAL.Entities
{
    public class OrderDetail: IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public short Quality { get; set; }
        public float Discount { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        [ForeignKey("Game")]
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
