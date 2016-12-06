using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace GameStore.Web.Helpers
{
    public static class QueryStringHelper
    {

        public static MvcHtmlString HiddenQueryStringParametersExceptFor(this HtmlHelper helper, params string[] keysToRemove)
        {
            var queryString = HttpContext.Current.Request.QueryString;
            string[] keys = queryString.AllKeys.Except(keysToRemove).ToArray();
            return HiddenQueryStringParameters(helper, keys);
        }

        public static MvcHtmlString HiddenQueryStringParameters(this HtmlHelper helper, params string[] keys)
        {

            var stringBuilder = new StringBuilder();
            var dictionary = QueryStringToDictionary(keys);
            foreach (var item in dictionary)
            {
                MvcHtmlString inputHidden = helper.Hidden(item.Key, item.Value);
                stringBuilder.Append(inputHidden);
            }
            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static RouteValueDictionary QueryStringParametersAsRouteValueDictionaryExceptFor(this HtmlHelper helper, params string[] keysToRemove)
        {
            var queryString = HttpContext.Current.Request.QueryString;
            var keys = queryString.AllKeys.Except(keysToRemove).ToArray();
            return QueryStringParametersAsRouteValueDictionary(helper, keys);
        }

        public static RouteValueDictionary QueryStringParametersAsRouteValueDictionary(this HtmlHelper helper, params string[] keys)
        {
            var dictionary = QueryStringToDictionary(keys);
            var routeValueDictionary = new RouteValueDictionary();
            foreach (var item in dictionary)
            {
                routeValueDictionary.Add(item.Key, item.Value);
            }
            return routeValueDictionary;
        }

        private static Dictionary<string, string> QueryStringToDictionary(string[] keys)
        {
            var dictionary = new Dictionary<string, string>();
            var queryString = HttpContext.Current.Request.QueryString;
            foreach (var key in keys)
            {
                var values = queryString.GetValues(key);
                if (values != null)
                {
                    if (values.Count() > 1)
                    {
                        for (int index = 0; index < values.Count(); index++)
                        {
                            var newKey = string.Format("{0}[{1}]", key, index);
                            dictionary.Add(newKey, values[index]);
                        }
                    }
                    else
                    {
                        dictionary.Add(key, values[0]);
                    }
                }
            }
            return dictionary;
        }
    }
}