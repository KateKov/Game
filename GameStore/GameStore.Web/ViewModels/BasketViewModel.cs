using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GameStore.Web.ViewModels
{
    public class BasketViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string GameKey { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string CustomerId { get; set; }
        public short UnitInStock { get; set; }
        [Required]
        [Range(1,500, ErrorMessage = "The quantity must be from 1 to 500")]
        public string Quantity { get; set; }
    }
}