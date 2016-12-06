using GameStore.BLL.Interfaces;
using GameStore.DAL.Enums;

namespace GameStore.BLL.DTO.Translation
{
    public class DTOTranslate : IDtoBase
    {
        public string Id { get; set; }
        public Language Language;
    }
}
