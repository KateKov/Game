using GameStore.BLL.DTO;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Interfaces.Services
{
    public interface IOrderService : IService<OrderDTO>
    {
        //Get 
        OrderDTO GetBasket(string username);
        OrderDTO GetOrders(string username);
        OrderDetailDTO GetOrderDetail(string gameKey, short quantity, bool isOrderExist);
        OrderFilterResultDTO GetOrdersByFilter(OrderFilterDTO filter, bool isHistory, int page, PageEnum pageSize);
        OrderDetailDTO GetExistedOrderDetail(OrderDetail orderDetailEntity, GameDTO game, short quantity);
        OrderDetailDTO GetNewOrderDetail(GameDTO game, short quantity);

        //Add
        void AddOrderDetail(string gameKey, short quantity, string username, bool isBasket);
        void AddOrder(OrderDetailDTO product, string username, bool isBasket);

        //Delete
        void DeleteOrder(string username, bool isBasket);

        //Edit
        void ChangeStatus(string username, bool isShipped);
        void Pay(string username);
        void UpdateOrder(string username, OrderDetailDTO product, bool isBasket);
        OrderDTO ConfirmeOrder(string id);
    }
}
