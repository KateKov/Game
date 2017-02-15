using System;
using System.Collections.Generic;

namespace GameStore.Web.Providers
{
    public static class GetDate
    {
        public static IEnumerable<int> GetAvailableMonths()
        {
            return new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        }

        public static IEnumerable<int> GetAvailableYears()
        {
            var availableYears = new List<int>();

            for (int index = 0; index < 10; index++)
            {
                availableYears.Add(DateTime.UtcNow.Year + index);
            }

            return availableYears;
        }
    }
}