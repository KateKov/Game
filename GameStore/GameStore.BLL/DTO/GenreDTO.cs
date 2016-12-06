using System.Collections.Generic;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class GenreDTO : IDtoBase, ITranslateDTONamed<GenreDTOTranslate>
    {     
        public string Id { get; set; }
        public string ParentId { get; set; }
        public List<string> GamesKey { get; set; }
        public List<string> GamesId { get; set; }
        public ICollection<GenreDTOTranslate> Translates { get; set; }
    }
}
