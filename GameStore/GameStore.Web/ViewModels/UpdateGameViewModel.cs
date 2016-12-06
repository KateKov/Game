﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GameStore.Web.ViewModels
{
    public class UpdateGameViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }
        [Required(ErrorMessage = "Game doesn't have Name")]
        [Display(Name = "Name")]
        [MaxLength(20, ErrorMessage ="The name can't be longer than 20 characters")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [MaxLength(1000, ErrorMessage = "The name can't be longer than 1000 characters")]
        public string Description { get; set; }

        public ICollection<string> Comments { get; set; }

        [Required(ErrorMessage = "Game doesn't have genres")]
        [Display(Name = "Genres")]
        public ICollection<string> GenresName { get; set; }
        [Display(Name = "Viewing")]
        public int Viewwing { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfAdding { get; set; }
        [Required(ErrorMessage = "Game doesn't have platform type")]
        [Display(Name = "Platform Types")]
        public ICollection<string> PlatformTypesName { get; set; }

        public string Key { get; set; }

        [Display(Name = "Price, UAH")]
        [Required(ErrorMessage = "Game doesn't have Price")]
        [Range(1, 100000)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Game doesn't have counts")]
        [Display(Name = "Units in Stock")]
        [Range(1, 10000)]
        public short UnitsInStock { get; set; }

        [Display(Name = "Company Name")]
        public string PublisherName { get; set; }

        [Required(ErrorMessage = "Not specified whether there is game available")]
        [Display(Name = "Discountinues")]
        public bool Discountinues { get; set; }

        public List<PublisherViewModel> AllPublishers { get; set; }
        public List<GenreViewModel> AllGenres { get; set; }
        public List<PlatformTypeViewModel> AllTypes { get; set; }
    }
}