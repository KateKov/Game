namespace GameStore.BLL.Interfaces.Services
{
    public interface INamedService<T, TD> : IService<T> where T : class, ITranslateDTONamed<TD>, IDtoBase, new()
        where TD : class, IDTOTranslate, new()
    {
        void DeleteByName(string name);
        T GetByName(string name);
    }
}