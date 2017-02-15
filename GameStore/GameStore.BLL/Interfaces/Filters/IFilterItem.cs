using System;

namespace GameStore.BLL.Interfaces
{
   public interface IFilterItem<in TEntity> where TEntity : class
    {
        Func<TEntity, object> Execute();
    }
}
