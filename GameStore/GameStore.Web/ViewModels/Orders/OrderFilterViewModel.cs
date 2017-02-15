using System;
using System.ComponentModel.DataAnnotations;
using GameStore.DAL.Enums;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Orders
{
    public class OrderFilterViewModel
    {
        [Display(ResourceType = typeof(GlobalRes), Name = "FilterOrder_DateFrom")]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "FilterOrder_DateTo")]
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; }

        public Filter FilterBy { get; set; }
    }
}