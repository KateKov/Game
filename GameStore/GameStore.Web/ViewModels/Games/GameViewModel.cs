using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Games
{
    public class GameViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }


        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ErrorName")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        public string Name { get; set; }



        [Display(ResourceType = typeof(GlobalRes), Name = "Description")]
        public string Description { get; set; }


        public ICollection<string> Comments { get; set; }


        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "GenresNameError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "GenresName")]
        public ICollection<string> GenresName { get; set; }



        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "PlatformTypeError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "PlatformTypesName")]
        public ICollection<string> PlatformTypesName { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Viewing")]
        public int Viewwing { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Picture")]
        [RegularExpression(@"^[\w]{3,}.(jpg|gif|png)", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "FilePathError")]
        public string FilePath { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(GlobalRes), Name = "FilterGame_DateOfAdding")]
        public DateTime DateOfAdding { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Game_Key")]
        public string Key { get; set; }



        [Display(ResourceType = typeof(GlobalRes), Name = "Price")]
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "PriceError")]
        public decimal Price { get; set; }



        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "UnitInStockError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "UnitsInStock")]
        public short UnitsInStock { get; set; }



        [Display(ResourceType = typeof(GlobalRes), Name = "CompanyName")]
        public string PublisherName { get; set; }



        [Display(ResourceType = typeof(GlobalRes), Name = "Publisher")]
        public string PublisherId { get; set; }

        public bool IsDeleted { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Discountinues")]
        public bool Discountinues { get; set; }

        public ICollection<string> OrderDetails { get; set; }
    }
}
