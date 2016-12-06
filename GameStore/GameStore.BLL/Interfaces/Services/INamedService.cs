using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Interfaces
{
    public interface INamedService<T, TD> where T : class, ITranslateDTONamed<TD>, IDtoBase, new() where TD : IDtoNamed
    {
        void DeleteByName(string name);
        T GetByName(string name);
    }
}
