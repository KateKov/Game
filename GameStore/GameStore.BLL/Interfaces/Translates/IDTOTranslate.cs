using GameStore.DAL.Enums;

namespace GameStore.BLL.Interfaces
{
    public interface IDTOTranslate : IDtoNamed
    {
        Language Language { get; set; }
    }
}
