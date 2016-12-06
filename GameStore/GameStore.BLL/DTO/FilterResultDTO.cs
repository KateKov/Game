using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
     public class FilterResultDTO
    {
        public IEnumerable<GameDTO> Games { get; set; }
        public int Count { get; set; }
    }
}
