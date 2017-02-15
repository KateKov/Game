using System.Collections.Generic;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class PlatformType : EntityBase, ITranslateNamed<PlatformTypeTranslate>
    {
        public virtual ICollection<PlatformTypeTranslate> Translates { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
