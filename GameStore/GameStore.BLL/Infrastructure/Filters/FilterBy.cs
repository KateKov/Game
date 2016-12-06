using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Enums;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Infrastructure.Filters
{
    public class FilterBy : IOperation<Game>
    {
        private readonly Filter _selectedFilter;

        public FilterBy(Filter selectedFilter)
        {
            _selectedFilter = selectedFilter;
        }

        public IQueryBuilder<Game> Execute(IQueryBuilder<Game> query)
        {
            Func<Game, object> condition = null;
            switch (_selectedFilter)
            {
                case Filter.Comments:
                    condition = game => game.Comments.Count;
                    break;
                case Filter.New:
                    condition = game => game.DateOfAdding.Ticks;
                    break;
                case Filter.Popularity:
                    condition = game => game.Viewing;
                    break;
                case Filter.PriceAsc:
                    condition = game => game.Price;
                    break;
                case Filter.PriceDesc:
                    condition = game => game.Price * -1;
                    break;
            }
            return condition != null ? query.OrderBy(condition) : query;
        }
    }
}
