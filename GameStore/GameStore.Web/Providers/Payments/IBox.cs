using System.Web.Mvc;
using GameStore.Web.ViewModels;
using System;

namespace GameStore.Web.Providers.Payments
{
    public class IBox : IPaymentStrategy
    {
        public ActionResult Pay(OrderViewModel order, Func<string, object, ViewResult> viewResult)
        {
            return viewResult("~/Views/Orders/Pay.cshtml", order);
        }
    }
}