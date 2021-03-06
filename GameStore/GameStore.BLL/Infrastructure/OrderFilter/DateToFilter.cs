﻿using System;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Infrastructure.OrderFilter
{
    public class DateToFilter : IOperation<Order>
    {
        private readonly DateTime _dateTo;

        public DateToFilter(DateTime dateTo)
        {
                _dateTo = dateTo;  
        }

        public IQueryBuilder<Order> Execute(IQueryBuilder<Order> query)
        {
            Func<Order, bool> condition = order => order.Date <= _dateTo.AddDays(1);
            return query.Where(condition);
        }
    }
}
