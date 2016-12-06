using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Infrastructure.OrderFilter
{
    public class DateFromFilter : IOperation<Order>
    {
        private readonly DateTime _dateFrom;
        public DateFromFilter(DateTime dateFrom)
        {
            _dateFrom = dateFrom;
        }

        public IQueryBuilder<Order> Execute(IQueryBuilder<Order> query)
        {
            Func<Order, bool> condition = order => order.Date >= _dateFrom;
            return query.Where(condition);
        }
    }
}
