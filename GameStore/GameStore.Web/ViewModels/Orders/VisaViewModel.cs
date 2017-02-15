using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.DAL.Enums;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Orders
{
    public class VisaViewModel
    {
        [HiddenInput]
        public string OrderId { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
           ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "CardHolderName")]
        public string CardHolderName { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
           ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
           ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Month")]
        public int MonthOfExpire { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
           ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "CardType")]
        public CardType CardType { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
           ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Year")]
        public int YearOfExpire { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
           ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "CVV2")]
        public int CvvCode { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Phone")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "EmailError")]
        public string Email { get; set; }

        public IEnumerable<int> Months { get; set; }

        public IEnumerable<int> Years { get; set; }
    }
}