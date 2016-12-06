

using System.Collections.Generic;

namespace GameStore.DAL.Interfaces
{
    public interface ITranslateNamed<T> : IEntityBase where T : IEntityNamed
    {
        ICollection<T> Translates { get; set; }
    }
}
