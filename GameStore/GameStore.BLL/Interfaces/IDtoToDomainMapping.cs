using System.Collections.Generic;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Interfaces
{
    public interface IDtoToDomainMapping
    {
        object AddEntities<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
            where TD : class, IDtoBase, new();

        object GetOrderDetails<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new() where TD : class, IDtoBase, new();
        object GetGames<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new() where TD : class, IDtoBase, new();
        object GetGenres<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new() where TD : class, IDtoBase, new();
        object GetTypes<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new() where TD : class, IDtoBase, new();
        object GetComments<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new() where TD : class, IDtoBase, new();
        object GetRole<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
         where TD : class, IDtoBase, new();
        object GetUser<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
            where TD : class, IDtoBase, new();

        Game GetGameWithPublisher(GameDTO gameDto);
        IEnumerable<Genre> GetGenres(GameDTO gameDto);
        IEnumerable<Genre> GetMongoGenres(GameDTO gameDto, List<Genre> genres);
    }
}
