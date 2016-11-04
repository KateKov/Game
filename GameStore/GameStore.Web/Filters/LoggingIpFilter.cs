using System.Web.Mvc;
using NLog;

namespace GameStore.Web.Filters
{
    public class LoggingIpFilter : FilterAttribute, IActionFilter
    {
        private static Logger Logger = LogManager.GetLogger("ips");

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.Url != null)
            {
                Logger.Debug($"Client info: IP = {filterContext.HttpContext.Request.UserHostAddress}; " +
                            $"URI = {filterContext.RequestContext.HttpContext.Request.Url.AbsoluteUri}");
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext) { }
    }
}