using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class OrderDetailDTO: IDtoBase
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public short Quality { get; set; }
        public float Discount { get; set; }
    }
}
