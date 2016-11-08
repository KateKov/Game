using System.Collections.Generic;
using GameStore.BLL.DTO;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Interfaces
{
    public interface IService
    {
        //Methods for games
        void AddGame(GameDTO gameDto);
        void EditGame(GameDTO gameDto);
        IEnumerable<GameDTO> GetGamesByGenreId(int genreId);
        IEnumerable<GameDTO> GetGamesByPlatformTypeId(int platformTypeId);
        IEnumerable<GameDTO> GetGamesByPlatformTypeName(string platformType);
        IEnumerable<GameDTO> GetGamesByGenreName(string genreName);

        //Methods for comments
        void AddComment(CommentDTO comment, string gameKey);
        IEnumerable<CommentDTO> GetCommentsByGameId(int gameId);
        IEnumerable<CommentDTO> GetCommentsByGameKey(string gameKey);

        //Generic methods
        void Add<T>(T model) where T : class, IDtoBase, new();
        T GetByKey<T>(string key) where T : class, IDtoBase, IDtoWithKey, new();
        T GetById<T>(int id) where T : class, IDtoBase, new();
        T GetByName<T>(string name) where T : class, IDtoBase, IDtoNamed, new();
        IEnumerable<T> GetAll<T>() where T : class, IDtoBase, new();    
        void DeleteByName<T>(string name) where T : class, IDtoBase, IDtoNamed, new();
        void DeleteById<T>(int id) where T : class, IDtoBase, IDtoNamed, new();
    }
}
