using System.Collections.Generic;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface IService
    {
        void AddGame(GameDTO gameDto);
        void EditGame(GameDTO gameDto);
        void DeleteGame(int id);
        void AddComment(CommentDTO comment, string gameKey);
        GameDTO GetGameById(int id);
        GameDTO GetGameByKey(string key);  
        IEnumerable<GameDTO> GetGames();
        IEnumerable<GameDTO> GetGamesByGenres(int genreId);
        IEnumerable<GameDTO> GetGamesByPlatformType(int platformTypeId);
        IEnumerable<CommentDTO> GetCommentsByGame(int gameId);
        IEnumerable<CommentDTO> GetCommentsByGameKey(string gameKey);
        IEnumerable<GameDTO> GetGamesByPlatformTypeName(string platformType);
        IEnumerable<GameDTO> GetGamesByGenresName(string genreName);
    }
}
