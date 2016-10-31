using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Interfaces
{
    public interface IService
    {
        void AddGame(GameDTO gameDto);
        void EditGame(GameDTO gameDto);
        void DeleteGame(int id);
        GameDTO GetGameById(int Id);
        DTO.GameDTO GetGameByKey(string Key);
        
        IEnumerable<GameDTO> GetGames();
        IEnumerable<GameDTO> GetGamesByGenres(int genreId);
        IEnumerable<GameDTO> GetGamesByPlatformType(int platformTypeId);
        void AddComment(CommentDTO comment, string gameKey);
        IEnumerable<CommentDTO> GetCommentsByGame(int gameId);

    }
}
