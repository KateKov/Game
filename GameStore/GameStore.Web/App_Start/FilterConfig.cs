using System.Web.Mvc;
using GameStore.Web.Filters;

namespace GameStore.Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LoggingIpFilter());
            filters.Add(new PerformanceFilter());
            filters.Add(new ErrorLoggingFilter());
        }
    }
}