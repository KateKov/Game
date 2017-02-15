using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO.Translation
{
    public class PublisherDTOTranslate : DTOTranslate, IDTOTranslateWithDescription
    {
        public string Description { get; set; }
    }
}
