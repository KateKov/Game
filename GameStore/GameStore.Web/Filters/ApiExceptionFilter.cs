using NLog;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace GameStore.Web.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public ApiExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(HttpActionExecutedContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var exception = filterContext.Exception;

            var httpCode = new HttpException(null, exception).GetHttpCode();
            filterContext.Response = new HttpResponseMessage((HttpStatusCode)httpCode);

            _logger.Error(exception, exception.Message);
        }
    }
}