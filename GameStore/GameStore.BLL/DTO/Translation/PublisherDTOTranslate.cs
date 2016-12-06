using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO.Translation
{
    public class PublisherDTOTranslate : DTOTranslate, IDtoNamed
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
