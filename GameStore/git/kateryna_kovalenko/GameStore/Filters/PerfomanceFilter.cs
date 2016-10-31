using System.Diagnostics;
using System.Web.Mvc;
using NLog;

namespace GameStore.Filters
{
    public class PerformanceFilter : FilterAttribute, IActionFilter
    {
        private static readonly Logger Logger = LogManager.GetLogger("performance");

        private readonly Stopwatch _sw;

        public PerformanceFilter()
        {
            _sw = new Stopwatch();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _sw.Start();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _sw.Stop();
            if (filterContext.RequestContext.HttpContext.Request.Url != null)
                Logger.Debug($"Performance info: URI = {filterContext.RequestContext.HttpContext.Request.Url.AbsoluteUri}; Time(ms) = {_sw.ElapsedMilliseconds}");
        }
    }
}