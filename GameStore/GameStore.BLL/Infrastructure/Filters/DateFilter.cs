using System;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Infrastructure.Filters
{
    public class DateFilter : IOperation<Game>
    {
        private readonly Date _selectedDate;

        public DateFilter(Date selectedDate)
        {
            _selectedDate = selectedDate;
        }

        public IQueryBuilder<Game> Execute(IQueryBuilder<Game> query)
        {
            Func<Game, bool> condition = null;
            switch (_selectedDate)
            {
                case Date.week:
                    condition = g => g.DateOfAdding >= DateTime.Today.AddDays(-7);
                    break;
                case Date.month:
                    condition = g => g.DateOfAdding >= DateTime.Today.AddMonths(-1);
                    break;
                case Date.year:
                    condition = g => g.DateOfAdding >= DateTime.Today.AddYears(-1);
                    break;
                case Date.twoyear:
                    condition = g => g.DateOfAdding >= DateTime.Today.AddYears(-2);
                    break;
                case Date.threeyear:
                    condition = g => g.DateOfAdding >= DateTime.Today.AddYears(-3);
                    break;
            }
            return condition != null ? query.Where(condition) : query;
        }
    }
}

