using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GameStore.DAL.Infrastracture;

namespace GameStore.DAL.Interfaces
{
    public interface IRepository<T> where T : class, new()
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Func<T, bool> where);
        QueryResult<T> GetAll(Query<T> query, bool isWithDeleted);
        T GetSingle(string id);
        int GetTotalNumber(bool isWithDeleted);
        int GetCount(Func<T, bool> where, bool isWithDeleted);
        bool IsExist(string id);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
    }
}
