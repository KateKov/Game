using System.Collections.Generic;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class Role : EntityBase, ITranslateNamed<RoleTranslate>
    {

        public bool IsDefault { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<RoleTranslate> Translates { get; set; }
    }
}
