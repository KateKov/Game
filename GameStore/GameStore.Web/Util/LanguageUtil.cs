using System.Web.Routing;

namespace GameStore.Web.Util
{
    public static class LanguageUtil
    {
        public static RouteValueDictionary CreateLanguageRoute(this RouteValueDictionary routes, string lang)
        {
            var res = new RouteValueDictionary();
            foreach (var key in routes.Keys)
            {
                res[key] = routes[key];
            }
            res["lang"] = lang;
            return res;
        }
    }
}