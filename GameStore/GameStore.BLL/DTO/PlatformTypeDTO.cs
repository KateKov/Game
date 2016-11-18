using System.Collections.Generic;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class PlatformTypeDTO : IDtoNamed
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> GameKey { get; set; }
        public  List<string> GameId { get; set; }
    }
}
