using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class OrderFilterResultDTO
    {
        public IEnumerable<OrderDTO> Orders { get; set; }
        public int Count { get; set; }
    }
}
