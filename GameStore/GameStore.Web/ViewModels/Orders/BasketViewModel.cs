using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Orders
{
    public class BasketViewModel
    {
        [HiddenInput(DisplayValue = false)]
        [Display(ResourceType = typeof(GlobalRes), Name ="Game_Key")]
        public string GameKey { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(ResourceType = typeof(GlobalRes), Name = "Order_CustomerId")]
        public string CustomerId { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "UnitsInStock")]
        public short UnitInStock { get; set; }

        [Required]
        [Range(1,500, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "QuantityError")]
        public string Quantity { get; set; }
    }
}