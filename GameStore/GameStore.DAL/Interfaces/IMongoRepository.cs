using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Infrastracture;

namespace GameStore.DAL.Interfaces
{
    public interface IMongoRepository<T> where T: class, IMongoEntity, new()
    {
        void Add(T entity);
        bool Edit(T entity);
        IEnumerable<T> GetAll();
        T GetSingle(string id);
        IEnumerable<TD> FindBy<TD>(Expression<Func<TD, bool>> predicate) where TD : class, IEntityBase, new();
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        void MakeOutdated(string id);
    }
}
