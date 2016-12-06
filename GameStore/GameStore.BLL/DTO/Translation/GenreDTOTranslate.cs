using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO.Translation
{
    public class GenreDTOTranslate : DTOTranslate, IDtoNamed
    {
        public string ParentName { get; set; }
        public string Name { get; set; }
    }
}
