namespace GameStore.Web.ViewModels
{
    public class OrderPaymentViewModel
    {
        public OrderViewModel Order { get; set; }

        public enum Payments
        {
            Bank,
            Box,
            Visa
        }

        public string Payment { get; set; }
    }
}