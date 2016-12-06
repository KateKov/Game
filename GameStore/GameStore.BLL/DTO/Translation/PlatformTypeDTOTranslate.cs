using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO.Translation
{
    public class PlatformTypeDTOTranslate : DTOTranslate, IDtoNamed
    {
        public string Name { get; set; }
    }
}
