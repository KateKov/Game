using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.ViewModels
{
    public class GameViewModel
    {
        [Required(ErrorMessage = "Game doesn't have Id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Game doesn't have Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public virtual ICollection<CommentViewModel> Comments { get; set; }
        [Required(ErrorMessage = "Game doesn't have genres")]
        [Display(Name = "Genres")]
        public virtual ICollection<GenreViewModel> Genres { get; set; }
        [Required(ErrorMessage = "Game doesn't have platform type")]
        [Display(Name = "Platform Types")]
        public virtual ICollection<PlatformTypeViewModel> PlatformTypes { get; set; }
        public string Key { get; set; }
        [Display(Name = "Price")]
        [Required(ErrorMessage = "Game doesn't have Price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Game doesn't have counts")]
        [Display(Name = "Units in Stock")]
        public short UnitsInStock { get; set; }
        [Display(Name = "Company Name")]
        public string PublisherName { get; set; }
        [Display(Name = "Publisher")]
        public int PublisherId { get; set; }
        [Required(ErrorMessage = "Not specified whether there is game available")]
        [Display(Name = "Discountinues")]
        public bool Discountinues { get; set; }
        public ICollection<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
