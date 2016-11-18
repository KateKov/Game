using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.ViewModels
{
    public class BasketViewModel
    {
        public string GameId { get; set; }
        public string CustomerId { get; set; }
        public short UnitInStock { get; set; }
        [Required]
        [Range(1,500, ErrorMessage = "The quantity must be from 1 to 500")]
        public string Quantity { get; set; }
    }
}