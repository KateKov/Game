using System.Collections.Generic;

namespace GameStore.BLL.Interfaces
{
    public interface ITranslateDTONamed<T> where T : IDTOTranslate
    {
        ICollection<T> Translates { get; set; }
    }
}
