using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Interfaces
{
   public interface IFilterItem<in TEntity> where TEntity : class
    {
        Func<TEntity, object> Execute();
    }
}
