using System;
using System.Linq;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Infrastructure.Filters
{
    public class SearchNameFilter : IOperation<Game>
    {
        private readonly string _name;
        public SearchNameFilter(string name)
        {
            if (name == null || name.Length < 3)
            {
                throw new ValidationException("There is no game with such name", string.Empty);
            }
            _name = name;
        }

        public IQueryBuilder<Game> Execute(IQueryBuilder<Game> query)
        {
            Func<Game, bool> condition = game => game != null && game.Translates!=null && game.Translates.Any(x=>x.Name.ToLower().Contains(_name.ToLower()));
            return query.Where(condition);
        }
    }
}
