using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.Web.App_LocalResources;
using GameStore.Web.ViewModels.Genres;
using GameStore.Web.ViewModels.PlatformTypes;
using GameStore.Web.ViewModels.Publishers;
using GameStore.Web.ViewModels.Translates;

namespace GameStore.Web.ViewModels.Games
{
    public class CreateGameViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        public IList<TranslateViewModelDescription> Translates { get; set; }
     
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "GenresNameError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "GenresName")]
        public ICollection<string> GenresName { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Viewing")]
        public int Viewing { get; set; }


        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(GlobalRes), Name = "Date")]
        public DateTime DateOfAdding { get; set; }


        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "PlatformTypeError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "PlatformTypesName")]
        public ICollection<string> PlatformTypesName { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Game_Key")]
        [RegularExpression(@"^[A-Za-z0-9_-]{1,50}", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "KeyError")]
        public string Key { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Price")]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "PriceError")]
        [Range(1, 100000, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "PriceRangeError")]
        public decimal Price { get; set; }


        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "UnitsInStockError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "UnitsInStock")]
        [Range(0, 10000, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "UnitsInStockError")]
        public short UnitsInStock { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "CompanyName")]
        public string PublisherName { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Picture")]
        [RegularExpression(@"^[\w]{3,}.(jpg|gif|png)", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "FilePathError")]
        public string FilePath { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Discountinues")]
        public bool Discountinues { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Description")]
        [MaxLength(1000, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "DescriptionRangeError")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ErrorName")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        [MaxLength(50, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "NameRangeError")]
        public string Name { get; set; }

        public IEnumerable<PublisherViewModel> AllPublishers { get; set; }
        public IEnumerable<GenreViewModel> AllGenres { get; set; }
        public IEnumerable<PlatformTypeViewModel> AllTypes { get; set; }
    }
}