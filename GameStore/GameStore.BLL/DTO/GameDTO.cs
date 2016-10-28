using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<GenreDTO> Genres { get; set; }
        public List<CommentDTO> Comments { get; set; }
        public List<PlatformTypeDTO> PlatformTypes { get; set; }
       
    }
}
