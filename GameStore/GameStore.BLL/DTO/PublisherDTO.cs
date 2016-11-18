using System.Collections.Generic;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class PublisherDTO : IDtoNamed
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HomePage { get; set; }
        public ICollection<string> Games { get; set; }
    }
}
