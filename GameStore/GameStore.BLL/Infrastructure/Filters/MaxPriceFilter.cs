using System;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Infrastructure.Filters
{
   public class MaxPriceFilter : IOperation<Game>
    {
        private readonly decimal _maxPrice;

        public MaxPriceFilter(decimal maxPrice)
        {
            _maxPrice = maxPrice;
        }

        public IQueryBuilder<Game> Execute(IQueryBuilder<Game> query)
        {
            Func<Game, bool> condition = game => game.Price <= _maxPrice;
            return query.Where(condition);
        }
    }
}
