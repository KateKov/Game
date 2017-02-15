using System;
using GameStore.DAL.Infrastracture;

namespace GameStore.BLL.Interfaces
{
    public interface IQueryBuilder<TEntity> where TEntity : class
    {
        IQueryBuilder<TEntity> Where(Func<TEntity, bool> where);

        IQueryBuilder<TEntity> OrderBy(Func<TEntity, object> orderBy);

        IQueryBuilder<TEntity> Take(int take);

        IQueryBuilder<TEntity> Skip(int skip);

        Query<TEntity> Done();
    }
}
