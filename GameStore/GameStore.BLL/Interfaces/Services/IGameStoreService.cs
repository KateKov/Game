namespace GameStore.BLL.Interfaces.Services
{
    public interface IGameStoreService
    {
        INamedService<T, TD> NamedService<T, TD>() where T : class, IDtoBase, ITranslateDTONamed<TD>, new() where TD: IDtoNamed;
        IWithKeyService<T> KeyService<T>() where T : class, IDtoWithKey, new();
        IService<T> GenericService<T>() where T : class, IDtoBase, new();
    }
}
