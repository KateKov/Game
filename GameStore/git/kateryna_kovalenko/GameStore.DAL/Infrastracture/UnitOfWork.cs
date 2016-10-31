using GameStore.DAL.EF;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;


namespace GameStore.DAL.Infrastracture
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly GameStoreContext context;
        private bool disposed;
        //private Dictionary<string, object> repositories;
        private Repository<Game> gameRepository;
        private Repository<PlatformType> platformTypeRepository;
        private  Repository<Genre> genreRepository;
        private  Repository<Comment> commentRepository;
        private readonly ILogger _logger;
        public UnitOfWork(GameStoreContext context, ILogger logger)
        {
            this.context = context;
            this._logger = logger;
        }

       

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        //public Repository<T> Repository<T>() where T : class, IEntityBase, new()
        //{
        //    if (repositories == null)
        //    {
        //        repositories = new Dictionary<string, object>();
        //    }

        //    var type = typeof(T).Name;

        //    if (!repositories.ContainsKey(type))
        //    {
        //        var repositoryType = typeof(Repository<>);
        //        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
        //        repositories.Add(type, repositoryInstance);
        //    }
        //    return (Repository<T>)repositories[type];
        //}

        public Repository<Game> GameRepository
        {
            get {
                if (gameRepository == null)
                    gameRepository = new Repository<Game>(context, _logger);
                return gameRepository;
            }
        }
        public Repository<Genre> GenreRepository
        {
            get
            {
                if (genreRepository == null)
                    genreRepository = new Repository<Genre>(context, _logger);
                return genreRepository;
            }
        }
        public Repository<Comment> CommentRepository
        {
            get
            {
                if (commentRepository == null)
                    commentRepository = new Repository<Comment>(context, _logger);
                return commentRepository;
            }
        }
        public Repository<PlatformType> PlatformTypeRepository
        {
            get
            {
                if (platformTypeRepository == null)
                    platformTypeRepository = new Repository<PlatformType>(context, _logger);
                return platformTypeRepository;
            }
        }
    }
    
    
}
