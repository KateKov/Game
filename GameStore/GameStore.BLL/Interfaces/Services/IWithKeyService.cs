namespace GameStore.BLL.Interfaces.Services
{
    public interface IWithKeyService<T> where T: class, IDtoWithKey, new()
    {
        T GetByKey(string key);
    }
}
