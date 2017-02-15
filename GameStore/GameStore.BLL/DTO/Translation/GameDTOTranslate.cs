using System.Collections.Generic;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO.Translation
{
    public class GameDTOTranslate : DTOTranslate, IDTOTranslateWithDescription
    {
        public string Description { get; set; }
        public string PublisherName { get; set; }
        public ICollection<string> GenresName { get; set; }
        public ICollection<string> PlatformTypesName { get; set; }
    }
}
