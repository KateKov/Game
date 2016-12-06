using System;

namespace GameStore.DAL.Infrastracture
{
   public class Query<TEntity>
    {
        public Func<TEntity, bool> Where { get; set; }
        public Func<TEntity, object> OrderBy { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
