using System.Web.Mvc;
using System;
using GameStore.Web.ViewModels.Orders;

namespace GameStore.Web.Providers.Payments
{
    public class Bank : IPaymentStrategy 
    {
        public ActionResult Pay(OrderViewModel order, Func<string, object, ViewResult> viewResult)
        {
            return viewResult("~/Views/Orders/Bank.cshtml", order);
        }
    }
}