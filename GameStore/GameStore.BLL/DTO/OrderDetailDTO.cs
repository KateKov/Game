using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class OrderDetailDTO: IDtoBase
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public short Quantity { get; set; }
        public string GameKey { get; set; }
        public float Discount { get; set; }
        public int OrderId { get; set; }
        public int GameId { get; set; }
    }
}
