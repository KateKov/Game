using System.Collections.Generic;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO.Translation
{
    public class GameDTOTranslate : DTOTranslate, IDtoNamed
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PublisherName { get; set; }
        public ICollection<string> GenresName { get; set; }
        public ICollection<string> PlatformTypesName { get; set; }
    }
}
