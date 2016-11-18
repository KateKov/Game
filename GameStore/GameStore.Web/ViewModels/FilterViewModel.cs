using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.Providers;

namespace GameStore.Web.ViewModels
{
    public class FilterViewModel
    {
        public List<CheckBox> ListGenres { get; set; }
        public ICollection<string> SelectedGenres { get; set; }
        public List<CheckBox> ListTypes { get; set; }
        public ICollection<string> SelectedType { get; set; }
        public List<CheckBox> ListPublishers { get; set; }
        public ICollection<string> SelectedPublishers { get; set; }

        public string SelectedFilter { get; set; }
        public enum Filter
        {
            [Display(Name = "Most popular")]
            Popularity = 1,
            [Display(Name = "Most commented")]
            Comments = 2,
            [Display(Name = "By price ascending")]
            PriceAsc = 3,
            [Display(Name = "By price descending")]
            PriceDesc = 4,
            [Display(Name = "New")]
            New
        }

        public decimal priceFrom { get; set; }
        public decimal priceTo { get; set; }

        public enum Date
        {
            [Display(Name = "last week")]
            week,
            [Display(Name = "last month")]
            month,
            [Display(Name = "2 year")]
            twoyear,
            [Display(Name = "3 year")]
            threeyear
        };

        public string NameSearch { get; set; }
    }
}