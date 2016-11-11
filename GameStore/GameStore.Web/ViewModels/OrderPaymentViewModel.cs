using System.Web.Mvc;
using GameStore.Web.Providers.Payments;

namespace GameStore.Web.ViewModels
{
    public class OrderPaymentViewModel
    {
        public OrderViewModel Order { get; set; }

        public enum Payments
        {
            Bank,
            IBox,
            Visa
        }

        public string Payment { get; set; }
    }
}