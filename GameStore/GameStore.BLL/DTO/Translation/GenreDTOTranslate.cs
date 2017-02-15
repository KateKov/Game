using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO.Translation
{
    public class GenreDTOTranslate : DTOTranslate, IDTOTranslate
    {
        public string ParentName { get; set; }
    }
}
