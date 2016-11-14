using System.Collections.Generic;
using GameStore.BLL.DTO;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Interfaces
{
    public interface IService
    {
        //Methods for games
        IEnumerable<GameDTO> GetGamesByGenreId(string genreId);
        IEnumerable<GameDTO> GetGamesByPlatformTypeId(string platformTypeId);
        IEnumerable<GameDTO> GetGamesByPlatformTypeName(string platformType);
        IEnumerable<GameDTO> GetGamesByGenreName(string genreName);

        //Methods for comments
        void AddComment(CommentDTO comment, string gameKey);
        IEnumerable<CommentDTO> GetCommentsByGameId(string gameId);
        IEnumerable<CommentDTO> GetCommentsByGameKey(string gameKey);

        //Methods for orders
        OrderDTO GetOrder(string id);
        OrderDTO GetOrderByCustomer(string id);
        OrderDTO ConfirmeOrder(string id);
        OrderDTO GetBusket(string customerId);
        IEnumerable<OrderDTO> GetOrders(string customerId);
        void AddToBusket(OrderDetailDTO product, string customerId);
        OrderDetailDTO GetOrderDetail(string gameId, short quantity, string customerId);
        void DeleteBusket(string customerId);

        //Generic methods
        void AddOrUpdate<T>(T model, bool isAdding) where T : class, IDtoBase, new();
        T GetByKey<T>(string key) where T : class, IDtoBase, IDtoWithKey, new();
        T GetById<T>(string id) where T : class, IDtoBase, new();
        T GetByName<T>(string name) where T : class, IDtoBase, IDtoNamed, new();
        IEnumerable<T> GetAll<T>() where T : class, IDtoBase, new();    
        void DeleteByName<T>(string name) where T : class, IDtoBase, IDtoNamed, new();
        void DeleteById<T>(string id) where T : class, IDtoBase, IDtoNamed, new();
    }
}
