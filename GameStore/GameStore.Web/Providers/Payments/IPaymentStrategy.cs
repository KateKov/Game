using System.Web.Mvc;
using GameStore.Web.ViewModels;

namespace GameStore.Web.Providers.Payments
{
    public interface IPaymentStrategy
    {
        ActionResult Pay(OrderViewModel order);
    }
}