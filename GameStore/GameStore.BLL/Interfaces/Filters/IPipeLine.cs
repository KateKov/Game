using GameStore.DAL.Infrastracture;

namespace GameStore.BLL.Interfaces
{
    public interface IPipeLine<TEntity> where TEntity : class
    {
        IPipeLine<TEntity> Register(IOperation<TEntity> operation);

        Query<TEntity> Execute();
    }
}
