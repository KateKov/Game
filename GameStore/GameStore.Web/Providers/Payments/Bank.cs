using System.Web.Mvc;
using GameStore.Web.ViewModels;
using Rotativa;

namespace GameStore.Web.Providers.Payments
{
    public class Bank : IPaymentStrategy 
    {
        public ActionResult Pay(OrderViewModel order)
        {
            return new ViewAsPdf("Bank", order);
        }
    }
}