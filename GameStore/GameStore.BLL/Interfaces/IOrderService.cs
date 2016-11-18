using GameStore.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Interfaces
{
    public interface IOrderService
    {
        OrderDTO ConfirmeOrder(string id);
        OrderDTO GetBusket(string customerId);
        OrderDTO GetOrders(string customerId);
        //void AddToBusket(OrderDetailDTO product, string customerId);
        void GetOrderDetail(string gameId, short quantity, string customerId);
        void DeleteBusket(string customerId);
    }
}
