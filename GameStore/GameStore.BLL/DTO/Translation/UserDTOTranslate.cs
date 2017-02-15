using System.Collections.Generic;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO.Translation
{
    public class UserDTOTranslate : DTOTranslate, IDTOTranslate
    {
        public ICollection<string> RolesName { get; set; }
        public string PublisherName { get; set; }
    }
}
