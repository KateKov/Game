using System;
using GameStore.DAL.Entities;

namespace GameStore.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        void Dispose(bool disposing);
        IRepository<Game> GameRepository { get; }
        IRepository<Genre> GenreRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<PlatformType> PlatformTypeRepository { get; }
        IRepository<T> Repository<T>() where T : class, IEntityBase, new();
    }
}
