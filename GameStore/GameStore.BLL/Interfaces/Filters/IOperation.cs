namespace GameStore.BLL.Interfaces
{
    public interface IOperation<TEntity> where TEntity : class
    {
        IQueryBuilder<TEntity> Execute(IQueryBuilder<TEntity> query);
    }
}
