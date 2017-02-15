using System.Collections.Generic;

namespace GameStore.BLL.Interfaces.Translates
{
    public interface ITranslateDTODescriptioned<T> where T : IDTOTranslateWithDescription
    {
        ICollection<T> Translates { get; set; }
    }
}