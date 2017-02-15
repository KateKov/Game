using System.Collections.Generic;

namespace GameStore.DAL.Interfaces
{
    public interface ITranslateNamed<T> where T : ITranslate
    {
        ICollection<T> Translates { get; set; }
    }
}
