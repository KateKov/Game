using System.Collections.Generic;

namespace GameStore.BLL.DTO
{
    public class OrderFilterResultDTO
    {
        public IEnumerable<OrderDTO> Orders { get; set; }
        public int Count { get; set; }
    }
}
