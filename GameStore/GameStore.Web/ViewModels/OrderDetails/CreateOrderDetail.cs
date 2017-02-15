using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;
using GameStore.Web.ViewModels.Games;

namespace GameStore.Web.ViewModels.OrderDetails
{
    public class CreateOrderDetail
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

        public IList<GameViewModel> AllGames { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Game_Key")]
        public string GameKey { get; set; }

        public string Username { get; set; }

        public int UnitsInStock { get; set; }
    }
}