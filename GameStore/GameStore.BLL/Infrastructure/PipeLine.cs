using System.Collections.Generic;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Infrastracture;

namespace GameStore.BLL.Infrastructure
{
    public class Pipeline<TEntity> : IPipeLine<TEntity> where TEntity : class
    {
        private readonly List<IOperation<TEntity>> _operations = new List<IOperation<TEntity>>();
        public IPipeLine<TEntity> Register(IOperation<TEntity> operation)
        {
            _operations.Add(operation);
            return this;
        }

        public Query<TEntity> Execute()
        {
            IQueryBuilder<TEntity> queryBuilder = new QueryBuilder<TEntity>();
            _operations.ForEach(operation => operation.Execute(queryBuilder));
            return queryBuilder.Done();
        }
    }
}
