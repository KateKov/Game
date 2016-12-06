using System.Collections.Generic;

namespace GameStore.DAL.Infrastracture
{
    public class QueryResult<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> List { get; set; }
        public int Count { get; set; }
    }
}
