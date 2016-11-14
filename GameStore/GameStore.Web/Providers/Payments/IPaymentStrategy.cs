using System.Web.Mvc;
using GameStore.Web.ViewModels;
using System;

namespace GameStore.Web.Providers.Payments
{
    public interface IPaymentStrategy
    {
        ActionResult Pay(OrderViewModel order, Func<string, object, ViewResult> viewResult);
    }
}