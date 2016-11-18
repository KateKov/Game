using GameStore.BLL.DTO;
using GameStore.DAL.Interfaces;
using System.Collections.Generic;

namespace GameStore.BLL.Interfaces
{
    public interface IGameService
    {
        IEnumerable<GameDTO> GetGamesByModelId<T>(string id) where T : class, IDtoBase, new();
        IEnumerable<GameDTO> GetGamesByNameOfProperty<T>(string name) where T : class, IDtoNamed, new();
        IEnumerable<GameDTO> MostViewed(IEnumerable<GameDTO> games);
        IEnumerable<GameDTO> MostCommented(IEnumerable<GameDTO> games);
        IEnumerable<GameDTO> GetGamesByPriceFilter(IEnumerable<GameDTO> gamesDto,bool isAsc);
        IEnumerable<GameDTO> GetNewGames(IEnumerable<GameDTO> gamesDto);
        IEnumerable<GameDTO> GetGamesByPriceRange(IEnumerable<GameDTO> gamesDto, decimal from, decimal to);
        IEnumerable<GameDTO> GetGamesByDate(IEnumerable<GameDTO> gamesDto, string date);
        IEnumerable<GameDTO> GetGamesByName(IEnumerable<GameDTO> gamesDto, string name);
        IEnumerable<GameDTO> Filter(IEnumerable<GameDTO> games, string filter);
        IEnumerable<GameDTO> GetGamesByNameOfProperty<T>(IEnumerable<GameDTO> gamesDto, string name)
            where T : class, IDtoNamed, new();
    }
}
