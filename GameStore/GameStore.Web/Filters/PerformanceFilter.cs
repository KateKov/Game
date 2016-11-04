using System.Diagnostics;
using System.Web.Mvc;
using NLog;

namespace GameStore.Web.Filters
{
    public class PerformanceFilter : FilterAttribute, IActionFilter
    {
        private static readonly Logger Logger = LogManager.GetLogger("performance");
        private readonly Stopwatch _stopWatch;
        public PerformanceFilter()
        {
            _stopWatch = new Stopwatch();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopWatch.Start();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _stopWatch.Stop();
            if (filterContext.RequestContext.HttpContext.Request.Url != null)
                Logger.Debug($"Performance info: URI = {filterContext.RequestContext.HttpContext.Request.Url.AbsoluteUri}; Time(ms) = {_stopWatch.ElapsedMilliseconds}");
        }
    }
}