using GameStore.BLL.DTO;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Interfaces
{
    public interface IOrderService
    {
        OrderDTO ConfirmeOrder(string id);
        OrderDTO GetBusket(string customerId);
        OrderDTO GetOrders();
        //void AddToBusket(OrderDetailDTO product, string customerId);
        void GetOrderDetail(string gameId, short quantity, string customerId);
        void DeleteBusket(string customerId);
        OrderFilterResultDTO GetOrdersByFilter(OrderFilterDTO filter, int page, PageEnum pageSize);
        void AddToBusket(OrderDetailDTO product, string customerId);
        void AddNewOrderToBasket(OrderDetailDTO product);
        OrderDetailDTO GetExistingOrder(OrderDetail orderDetailEntity, GameDTO game, short quantity);
        void PayOrder();
    }
}
