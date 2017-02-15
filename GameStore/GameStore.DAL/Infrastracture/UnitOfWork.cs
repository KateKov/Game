using System;
using System.Collections.Generic;
using GameStore.DAL.EF;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Mapping;
using GameStore.DAL.MongoEntities;

namespace GameStore.DAL.Infrastracture
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly GameStoreContext _context;
        private bool _disposed;
        private Dictionary<string, object> repositories;

        public UnitOfWork(GameStoreContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public IRepository<T> Repository<T>() where T : class, IEntityBase, new()
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var mongoType = (GetMongo<T>.IsMongo()) ? GetMongo<T>.GetMongoType() : new MongoBase();
                var repositoryType = typeof(CommonRepository<,>);
                var repositoryInstance =
                    new Lazy<IRepository<T>>(
                        () =>
                            (IRepository<T>)
                            Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T), mongoType.GetType()),
                                _context)).Value;

                repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>) repositories[type];
        }
    }
}