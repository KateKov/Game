using System.Collections.Generic;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Translates;

namespace GameStore.BLL.DTO
{
    public class PublisherDTO : IDtoBase, ITranslateDTODescriptioned<PublisherDTOTranslate>, ITranslateDTONamed<PublisherDTOTranslate>
    {
        public string Id { get; set; }
        public string HomePage { get; set; }
        public List<string> GamesKey { get; set; }
        public ICollection<PublisherDTOTranslate> Translates { get; set; }
        public bool IsDeleted { get; set; }
    }
}
