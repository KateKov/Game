using System;

namespace GameStore.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        void Dispose(bool disposing);
        IRepository<T> Repository<T>() where T : class, IEntityBase, new();
    }
}
