using System.Web.Mvc;
using GameStore.Web.ViewModels;

namespace GameStore.Web.Providers.Payments
{
    public class Visa : Controller, IPaymentStrategy
    {
        public ActionResult Pay(OrderViewModel order)
        {
            return View("Visa");
        }
    }
}