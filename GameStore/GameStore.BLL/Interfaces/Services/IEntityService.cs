using System.Collections.Generic;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Interfaces
{
    public interface IEntityService<T, TD> where T: class, IEntityBase, new() where TD: class, IDtoBase, new()
    {
        void AddEntity(TD model);
        void EditEntity(TD model);
        IEnumerable<T> GetAllEntities();
        TK GetEntityByKey<TK>(string key) where TK : class, IEntityWithKey, new();
        T GetEntityById(string id);
        void DeleteEntityById(string id);
    }
}
