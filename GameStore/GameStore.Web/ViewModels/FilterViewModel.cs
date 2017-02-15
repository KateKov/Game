using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.Providers;
using GameStore.DAL.Enums;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels
{
    public class FilterViewModel
    {
        [Display(ResourceType = typeof(GlobalRes), Name = "Genres")]
        public IEnumerable<CheckBox> ListGenres { get; set; }
        public IEnumerable<CheckBox> SelectedGenres { get; set; }
        public IEnumerable<string> SelectedGenresName { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "PlatformTypes")]
        public IEnumerable<CheckBox> ListTypes { get; set; }
        public IEnumerable<CheckBox> SelectedType { get; set; }
        public IEnumerable<string> SelectedTypesName { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Publishers")]
        public IEnumerable<CheckBox> ListPublishers { get; set; }
        public IEnumerable<CheckBox> SelectedPublishers { get; set; }
        public IEnumerable<string> SelectedPublishersName { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "FilterGame_DateOfAdding")]
        public Date DateOfAdding { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "FilterGame_FilterBy")]
        public Filter FilterBy { get; set; }



        [Display(ResourceType = typeof(GlobalRes), Name = "FilterGame_MinPrice")]
        [Range(0, 100000, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "PriceRangeError")]
        public decimal? MinPrice { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "FilterGame_MaxPrice")]
        [Range(0, 100000, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "PriceRangeError")]
        public decimal? MaxPrice { get; set; }



        [MaxLength(50, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "NameRangeError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        public string Name { get; set; }
    }
}