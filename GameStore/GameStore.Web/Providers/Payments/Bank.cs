using System.Web.Mvc;
using GameStore.Web.ViewModels;
using Rotativa;
using System;

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