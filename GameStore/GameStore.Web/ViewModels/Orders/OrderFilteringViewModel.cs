using System.Collections.Generic;
using GameStore.DAL.Enums;

namespace GameStore.Web.ViewModels.Orders
{
    public class OrderFilteringViewModel
    {
        public IEnumerable<OrderViewModel> Orders { get; set; }
        public OrderFilterViewModel Filter { get; set; }
        public int Page { get; set; }
        public PageEnum PageSize { get; set; }
        public int TotalItemsCount { get; set; }
    }
}