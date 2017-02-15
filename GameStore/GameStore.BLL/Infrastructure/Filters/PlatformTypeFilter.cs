using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.DAL.Entities;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.Infrastructure.Filters
{
    class PlatformTypeFilter : IOperation<Game>
    {
        private readonly IEnumerable<string> _selectedPlatforms;
        public PlatformTypeFilter(IEnumerable<string> selectedPlatforms)
        {
            var platforms = selectedPlatforms as IList<string> ?? selectedPlatforms.ToList();
            if (!platforms.Any())
            {
                throw new ValidationException("There is no platform type", "Type");
            }
            _selectedPlatforms = platforms;
        }

        public IQueryBuilder<Game> Execute(IQueryBuilder<Game> filter)
        {
            Func<Game, bool> condition = x => x.PlatformTypes.Any(
                   g => g.Translates!=null && _selectedPlatforms.Any(name => g.Translates.Any(t=>t.Name==name)));
            return filter.Where(condition);
        }
    }
}
