using System.Collections.Generic;
using System.Security.Policy;
using AutoMapper.Configuration.Conventions;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class GameDTO : IDtoBase, IDtoWithKey, IDtoNamed
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public short UnitsInStock { get; set; }
        public bool Discountinues { get; set; }
        public string PublisherName { get; set; }
        public string PublisherId { get; set; }
        public ICollection<OrderDetailDTO> OrderDetails { get; set; }
        public ICollection<GenreDTO> Genres { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }
        public ICollection<PlatformTypeDTO> PlatformTypes { get; set; }
    }
}
