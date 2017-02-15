using System.Web.Mvc;
using System;
using GameStore.Web.ViewModels.Orders;

namespace GameStore.Web.Providers.Payments
{
    public interface IPaymentStrategy
    {
        ActionResult Pay(OrderViewModel order, Func<string, object, ViewResult> viewResult);
    }
}