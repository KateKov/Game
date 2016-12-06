using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.BLL.Infrastructure.Filters
{
    public class GenreFilter : IOperation<Game>
    {
        private readonly IEnumerable<string> _selectedGenres;
        public GenreFilter(IEnumerable<string> selectedGenres)
        {
            var genresName = selectedGenres as IList<string> ?? selectedGenres.ToList();
            if (genresName.Any() == false)
            {
                throw new ValidationException("There is no genres", string.Empty);
            }
            _selectedGenres = genresName;
        }

        public IQueryBuilder<Game> Execute(IQueryBuilder<Game> query)
        {
            Func<Game, bool> condtion = x => x.Genres.Any(g => g.Translates!=null && _selectedGenres.Any(name => g.Translates.Any(t=>t.Name==name)));
            return query.Where(condtion);
        }

    }
}
