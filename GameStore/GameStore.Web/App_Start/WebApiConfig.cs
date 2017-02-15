using System.Net.Http.Formatting;
using System.Web.Http;
using GameStore.Web.Filters;
using NLog;

namespace GameStore.Web
{
    public static class WebApiConfig
    {
        private const string DefaultLanguage = "en";

        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new ApiExceptionFilter(LogManager.GetLogger("errors")));

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.Add(new XmlMediaTypeFormatter());

            config.Formatters.XmlFormatter.AddQueryStringMapping("type", "xml", "text/xml");
            config.Formatters.JsonFormatter.AddQueryStringMapping("type", "json", "application/json");


            config.Filters.Add(new ApiExceptionFilter(LogManager.GetLogger("errors")));

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}",
                defaults: new { lang = DefaultLanguage });

            config.EnsureInitialized();
            config.EnableCors();
        }
    }
}