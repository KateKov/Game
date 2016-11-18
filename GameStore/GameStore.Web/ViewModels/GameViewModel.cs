using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GameStore.Web.ViewModels
{
    public class GameViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }
        [Required(ErrorMessage = "Game doesn't have Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public ICollection<string> Comments { get; set; }

        [Required(ErrorMessage = "Game doesn't have genres")]
        [Display(Name = "Genres")]
        public ICollection<string> GenresName { get; set; }

        [Required(ErrorMessage = "Game doesn't have platform type")]
        [Display(Name = "Platform Types")]
        public ICollection<string> PlatformTypesName { get; set; }
        [Display(Name = "Viewing")]
        public int Viewwing { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfAdding { get; set; }
        public string Key { get; set; }

        [Display(Name = "Price, UAH")]
        [Required(ErrorMessage = "Game doesn't have Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Game doesn't have counts")]
        [Display(Name = "Units in Stock")]
        public short UnitsInStock { get; set; }

        [Display(Name = "Company Name")]
        public string PublisherName { get; set; }

        [Display(Name = "Publisher")]
        public string PublisherId { get; set; }

        [Required(ErrorMessage = "Not specified whether there is game available")]
        [Display(Name = "Discountinues")]
        public bool Discountinues { get; set; }

        public ICollection<string> OrderDetails { get; set; }
    }
}
