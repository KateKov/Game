using System.Web.Mvc;
using System;
using GameStore.Web.ViewModels.Orders;

namespace GameStore.Web.Providers.Payments
{
    public class CardPay : IPaymentStrategy
    {
        public ActionResult Pay(OrderViewModel order, Func<string, object, ViewResult> viewResult)
        {
            var visa = new VisaViewModel()
            {
                OrderId = order.Id,
                Months = GetDate.GetAvailableMonths(),
                Years = GetDate.GetAvailableYears()
            };
            return viewResult("~/Views/Orders/CardPay.cshtml", visa);
        }
    }
}