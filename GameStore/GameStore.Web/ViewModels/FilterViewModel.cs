using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.Providers;
using GameStore.DAL.Enums;

namespace GameStore.Web.ViewModels
{
    public class FilterViewModel
    {
        [DisplayName("Genres")]
        public IEnumerable<CheckBox> ListGenres { get; set; }
        public IEnumerable<CheckBox> SelectedGenres { get; set; }
        public IEnumerable<string> SelectedGenresName { get; set; }
        [DisplayName("Types")]
        public IEnumerable<CheckBox> ListTypes { get; set; }
        public IEnumerable<CheckBox> SelectedType { get; set; }
        public IEnumerable<string> SelectedTypesName { get; set; }
        [DisplayName("Publishers")]
        public IEnumerable<CheckBox> ListPublishers { get; set; }
        public IEnumerable<CheckBox> SelectedPublishers { get; set; }
        public IEnumerable<string> SelectedPublishersName { get; set; }
        public Date DateOfAdding { get; set; }
        public Filter FilterBy { get; set; }

        [DisplayName("From price")]
        [Range(0, 100000, ErrorMessage = "The price from isn't correct")]
        public decimal? MinPrice { get; set; }
        [DisplayName("To price")]
        [Range(0, 100000, ErrorMessage = "The price to isn't correct")]
        public decimal? MaxPrice { get; set; }

        [MaxLength(20, ErrorMessage = "The length of name is invalid")]
        public string Name { get; set; }
    }
}