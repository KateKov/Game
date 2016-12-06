using System.Collections.Generic;

namespace GameStore.BLL.Interfaces
{
    public interface IService<T> where T: class, IDtoBase, new()
    {
        void AddEntity(T model);
        void EditEntity(T model);
        T GetById(string id);
        IEnumerable<T> GetAll();
        void DeleteById(string id);
        void AddOrUpdate(T model, bool isAdding);
    }
}
