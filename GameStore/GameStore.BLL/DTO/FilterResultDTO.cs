using System.Collections.Generic;

namespace GameStore.BLL.DTO
{
     public class FilterResultDTO
    {
        public IEnumerable<GameDTO> Games { get; set; }
        public int Count { get; set; }
    }
}
