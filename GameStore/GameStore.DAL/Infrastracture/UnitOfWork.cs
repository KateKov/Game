using System;
using GameStore.DAL.EF;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using NLog;
using System.Collections.Generic;
using NLog.LayoutRenderers.Wrappers;

namespace GameStore.DAL.Infrastracture
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly GameStoreContext _context;
        private readonly Repository<Game> _gameRepository;
        private readonly Repository<Genre> _genreRepository;
        private readonly Repository<Comment> _commentRepository;
        private readonly Repository<PlatformType> _typeRepository;
        private readonly ILogger _logger;
        private bool disposed;
        private Dictionary<string, object> repositories;

        public UnitOfWork(GameStoreContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            _gameRepository = new Repository<Game>(_context);
            _genreRepository = new Repository<Genre>(_context);
            _commentRepository = new Repository<Comment>(_context);
            _typeRepository = new Repository<PlatformType>(_context);
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
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            disposed = true;
        }

        public IRepository<Game> GameRepository
        {
            get { return _gameRepository ?? new Repository<Game>(_context); }
        } 

            public IRepository<Genre> GenreRepository
        {
            get { return _genreRepository ?? new Repository<Genre>(_context); }
        }

        public IRepository<Comment> CommentRepository
        {
            get { return _commentRepository ?? new Repository<Comment>(_context); }
        }

        public IRepository<PlatformType> PlatformTypeRepository
        {
            get { return _typeRepository ?? new Repository<PlatformType>(_context); }
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
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                repositories.Add(type, repositoryInstance);
            }
            return (IRepository<T>)repositories[type];
        }
    }
}   

