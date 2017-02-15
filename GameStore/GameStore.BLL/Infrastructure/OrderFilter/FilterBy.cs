using System;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Infrastructure.OrderFilter
{
    public class FilterBy : IOperation<Order>
    {

        private readonly Filter _selectedFilter;

        public FilterBy(Filter selectedFilter)
        {
            _selectedFilter = selectedFilter;
        }

        public IQueryBuilder<Order> Execute(IQueryBuilder<Order> query)
        {
            Func<Order, object> condition = null;
            switch (_selectedFilter)
            {
                case Filter.New:
                    condition = order => order.Date.Ticks * -1;
                    break;
                
                case Filter.PriceAsc:
                    condition = order => order.Sum;
                    break;
                case Filter.PriceDesc:
                    condition =  order => order.Sum  * -1;
                    break;
            }
            return condition != null ? query.OrderBy(condition) : query;
        }
    }
}
