using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Interfaces
{
    public interface IGameService : INamedService<GameDTO, GameDTOTranslate>, IService<GameDTO>
    {
        FilterResultDTO GetAllByFilter(FilterDTO filter, bool isWithDeleted, int page, PageEnum pageSize);
        GameDTO GetByKey(string key);
        void DeleteByKey(string key);
    }
}
