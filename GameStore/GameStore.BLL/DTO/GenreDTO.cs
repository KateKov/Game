using System.Collections.Generic;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class GenreDTO : IDtoBase, IDtoNamed
    {     
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string ParentName { get; set; }
        public List<string> GamesKey { get; set; }
        public List<string> GamesId { get; set; }
    }
}
