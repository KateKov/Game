using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class OrderDetailDTO: IDtoBase
    {
        public string Id { get; set; }
        public decimal Price { get; set; }
        public short Quantity { get; set; }
        public string GameKey { get; set; }
        public float Discount { get; set; }
        public string OrderId { get; set; }
        public string GameId { get; set; }
    }
}
