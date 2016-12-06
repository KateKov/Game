using System.Collections.Generic;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class PlatformTypeDTO : IDtoBase, ITranslateDTONamed<PlatformTypeDTOTranslate>
    {
        public string Id { get; set; }
        public List<string> GameKey { get; set; }
        public  List<string> GameId { get; set; }
        public ICollection<PlatformTypeDTOTranslate> Translates { get; set; }
    }
}
