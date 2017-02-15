using System;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Infrastructure.Filters
{
    public class MinPriceFilter : IOperation<Game>
    {
        private readonly decimal _minPrice;

        public MinPriceFilter(decimal minPrice)
        {
            _minPrice = minPrice;
        }

        public IQueryBuilder<Game> Execute(IQueryBuilder<Game> query)
        {
            Func<Game, bool> condition = game => game.Price >= _minPrice;
            return query.Where(condition);
        }
    }
}
