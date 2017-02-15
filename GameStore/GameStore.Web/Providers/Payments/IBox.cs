using System.Web.Mvc;
using System;
using GameStore.Web.ViewModels.Orders;

namespace GameStore.Web.Providers.Payments
{
    public class Box : IPaymentStrategy
    {
        public ActionResult Pay(OrderViewModel order, Func<string, object, ViewResult> viewResult)
        {
            return viewResult("~/Views/Orders/Pay.cshtml", order);
        }
    }
}