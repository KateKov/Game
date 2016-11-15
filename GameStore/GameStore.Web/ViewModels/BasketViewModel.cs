namespace GameStore.Web.ViewModels
{
    public class BasketViewModel
    {
        public string GameId { get; set; }
        public string CustomerId { get; set; }
        public short UnitInStock { get; set; }
        public string Quantity { get; set; }
    }
}