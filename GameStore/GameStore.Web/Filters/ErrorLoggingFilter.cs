using System.Web.Mvc;
using NLog;

namespace GameStore.Web.Filters
{
    public class ErrorLoggingFilter : FilterAttribute, IExceptionFilter
   {
        private static Logger Logger = LogManager.GetLogger("errors");
 
         public void OnException(ExceptionContext filterContext)
         {
             if (filterContext != null)
             {
                 Logger.Error("Unhandled exception: {0} | StackTrace: {1} ",
                     filterContext.Exception.Message,
                     filterContext.Exception.StackTrace);
             }
         }
    }
}