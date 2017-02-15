using System.Collections.Generic;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Enums;

namespace GameStore.BLL.DTO
{
    public class RoleDTO : IDtoBase, ITranslateDTONamed<RoleDTOTranslate>
    {
        public string Id { get; set; }

        public Language Language { get; set; }

        public bool IsDefault { get; set; }

        public ICollection<string> Users { get; set; }

        public ICollection<RoleDTOTranslate> Translates { get; set; }
        public bool IsDeleted { get; set; }
    }
}
