using System.Web.Mvc;
using GameStore.Web.ViewModels;
using System;

namespace GameStore.Web.Providers.Payments
{
    public class Visa : IPaymentStrategy
    {
        public ActionResult Pay(OrderViewModel order, Func<string, object, ViewResult> viewResult)
        {
            return viewResult("~/Views/Orders/Visa.cshtml", order);
        }
    }
}