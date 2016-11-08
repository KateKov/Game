using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.ViewModels
{
    public class GameViewModel
    {
        [Required(ErrorMessage = "Game doesn't have Id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Game doesn't have Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<CommentViewModel> Comments { get; set; }
        [Required(ErrorMessage = "Game doesn't have genres")]
        public virtual ICollection<GenreViewModel> Genres { get; set; }
        [Required(ErrorMessage = "Game doesn't have platform type")]
        public virtual ICollection<PlatformTypeViewModel> PlatformTypes { get; set; }
        public string Key { get; set; }
        [Required(ErrorMessage = "Game doesn't have Price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Game doesn't have counts")]
        public short UnitsInStock { get; set; }
        public string PublisherName { get; set; }
        public int PublisherId { get; set; }
        [Required(ErrorMessage = "Not specified whether there is game available")]
        public bool Discountinues { get; set; }
        public ICollection<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
