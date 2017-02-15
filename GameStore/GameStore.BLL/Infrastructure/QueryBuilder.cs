using System;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Infrastracture;

namespace GameStore.BLL.Infrastructure
{
    public class QueryBuilder<TEntity> : IQueryBuilder<TEntity> where TEntity : class
    {
        private readonly Query<TEntity> _query;

        public QueryBuilder()
        {
            _query = new Query<TEntity>();
        }

        public IQueryBuilder<TEntity> Where(Func<TEntity, bool> where)
        {
            if (_query.Where == null)
            {
                _query.Where = where;
            }
            else
            {
                var otherConditions = _query.Where;
                _query.Where = x => otherConditions(x) && where(x);
            }

            return this;
        }

        public IQueryBuilder<TEntity> OrderBy(Func<TEntity, object> orderBy)
        {
            _query.OrderBy = orderBy;
            return this;
        }

        public IQueryBuilder<TEntity> Take(int take)
        {
            _query.Take = take;
            return this;
        }

        public IQueryBuilder<TEntity> Skip(int skip)
        {
            _query.Skip = skip;
            return this;
        }

        public Query<TEntity> Done()
        {
            return _query;
        }
    }
}
