using System.Collections.Generic;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class GameDTO : IDtoBase
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public short UnitsInStock { get; set; }
        public bool Discountinues { get; set; }
        public ICollection<OrderDetailDTO> OrderDetails { get; set; }
        public ICollection<GenreDTO> Genres { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }
        public ICollection<PlatformTypeDTO> PlatformTypes { get; set; }
    }
}
