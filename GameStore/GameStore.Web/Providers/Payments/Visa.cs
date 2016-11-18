using System.Web.Mvc;
using GameStore.Web.ViewModels;
using System;

namespace GameStore.Web.Providers.Payments
{
    public class Visa : IPaymentStrategy
    {
        public ActionResult Pay(OrderViewModel order, Func<string, object, ViewResult> viewResult)
        {
            var visa = new VisaViewModel() {Order = order};
            return viewResult("~/Views/Orders/Visa.cshtml", visa);
        }
    }
}