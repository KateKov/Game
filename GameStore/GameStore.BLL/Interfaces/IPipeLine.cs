using GameStore.DAL.Infrastracture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Interfaces
{
    public interface IPipeLine<TEntity> where TEntity : class
    {
        IPipeLine<TEntity> Register(IOperation<TEntity> operation);

        Query<TEntity> Execute();
    }
}
