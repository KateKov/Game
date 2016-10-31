using GameStore.DAL.Entities;
using GameStore.DAL.Infrastracture;
using GameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {

       

        void Save();
        void Dispose(bool disposing);
        //Repository<T> Repository<T>() where T : class, IEntityBase, new();
        Repository<Game> GameRepository { get; }
        Repository<Genre> GenreRepository { get; }
        Repository<PlatformType> PlatformTypeRepository { get; }
        Repository<Comment> CommentRepository { get; }


    }
}
