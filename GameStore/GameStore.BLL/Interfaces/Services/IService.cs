using System.Collections.Generic;

namespace GameStore.BLL.Interfaces
{
    public interface IService<T> where T: class, IDtoBase, new()
    {
        void AddEntity(T model);

        void EditEntity(T model);

        T GetById(string id);

        IEnumerable<T> GetAll(bool isWithDeleted);

        void DeleteById(string id);

        bool IsExist(string id);

        int GetTotalNumber(bool isWithDeleted);
    }
}
