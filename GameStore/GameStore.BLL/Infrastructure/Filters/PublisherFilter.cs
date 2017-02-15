using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Infrastructure.Filters
{
    internal class PublisherFilter : IOperation<Game>
    {
        private readonly IEnumerable<string> _selectedPublisher;
        public PublisherFilter(IEnumerable<string> selectedPublisher)
        {
            var publisher = selectedPublisher as IList<string> ?? selectedPublisher.ToList();
            if (publisher.Any() == false)
            {
                throw new ValidationException("There is no publisher", "Publisher");
            }

            _selectedPublisher = publisher;
        }

        public IQueryBuilder<Game> Execute(IQueryBuilder<Game> query)
        {
            Func<Game, bool> condition =
                p => _selectedPublisher.Any(name =>p.Publisher!=null && p.Publisher.Translates!=null && p.Publisher.Translates.Any(t=>t.Name==name));
            return query.Where(condition);
        }
    }
}
