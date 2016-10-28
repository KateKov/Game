using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Interfaces
{
    public interface IGameService
    {
        void AddGame(GameDTO gameDto);
        void EditGame(GameDTO gameDto);
        void DeleteGame(GameDTO gameDto);
        GameDTO GetGameById(int Id);
        DTO.GameDTO GetGameByKey(string Key);
        
        List<GameDTO> GetGames();
        List<GameDTO> GetGamesByGenres(int genreId);
        List<GameDTO> GetGamesByPlatformType(int platformTypeId);
    }
}
