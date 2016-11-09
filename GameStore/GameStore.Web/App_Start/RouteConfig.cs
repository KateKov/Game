using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "GamesDefault",
                url: "games",
                defaults: new { controller = "Games", action = "Index" });
            //routes.MapRoute(
            //    name: "CommentsDefault",
            //    url: "game/{id}/{action}",
            //    defaults: new { controller = "Comments" },
            //    constraints: new { id = @"^[A-Za-z0-9_-]{1,20}", action = @"\w+" });
            //routes.MapRoute(
            //    name: "Comments",
            //    url: "game/{id}/{action}",
            //    defaults: new { controller = "Comments", action = @"Details"},
            //    constraints: new { id = @"^[A-Za-z0-9_-]{1,20}"});
            routes.MapRoute(
                name: "Games",
                url: "games/{action}",
                defaults: new { controller = "Games", action = @"\w+" });
            routes.MapRoute(
                name: "Default",
                url: string.Empty,
                defaults: new { controller = "Games", action = "Index", id = string.Empty });
        }
    }
}
