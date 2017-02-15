using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Orders
{
    public class OrderDetailViewModel
    {
        [Required]
        public string Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Price")]
        public decimal Price { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Quantity")]
        public short Quantity { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Discount")]
        public float Discount { get; set; }

        public string OrderId { get; set; }

        [Required]
        public string GameId { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Game_Key")]
        public string GameKey { get; set; }
    }
}