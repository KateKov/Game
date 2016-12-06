using GameStore.BLL.DTO;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Interfaces
{
    public interface IGameService
    {
        FilterResultDTO GetAllByFilter(FilterDTO filter, int page, PageEnum pageSize);
    }
}
