using System.Collections.Generic;
using GameStore.BLL.DTO;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Interfaces
{
    public interface IService
    {

        //Generic methods
        void AddOrUpdate<T>(T model, bool isAdding) where T : class, IDtoBase, new();
        T GetByKey<T>(string key) where T : class, IDtoBase, IDtoWithKey, new();
        T GetByName<T>(string name) where T : class, IDtoBase, IDtoNamed, new();
        IEnumerable<T> GetAll<T>() where T : class, IDtoBase, new();    
        void DeleteById<T>(string id) where T : class, IDtoBase, IDtoNamed, new();
    }
}
