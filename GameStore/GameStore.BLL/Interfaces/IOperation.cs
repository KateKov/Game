using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Interfaces
{
    public interface IOperation<TEntity> where TEntity : class
    {
        IQueryBuilder<TEntity> Execute(IQueryBuilder<TEntity> query);
    }
}
