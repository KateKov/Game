using System.Collections.Generic;

namespace GameStore.BLL.DTO
{
    public class GenreDTO
    {     
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public virtual ICollection<GenreDTO> ChildGenres { get; set; } 
    }
}
