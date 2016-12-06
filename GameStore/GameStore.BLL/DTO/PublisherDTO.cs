using System.Collections.Generic;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class PublisherDTO : IDtoBase, ITranslateDTONamed<PublisherDTOTranslate>
    {
        public string Id { get; set; }
        public string HomePage { get; set; }
        public ICollection<PublisherDTOTranslate> Translates { get; set; }
    }
}
