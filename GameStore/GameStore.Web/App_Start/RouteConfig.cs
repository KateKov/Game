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
            routes.MapRoute(
                name: "CommentsDefault",
                url: "game/{key}/{action}",
                defaults: new { controller = "Comments" },
                constraints: new { key = @"^[A-Za-z0-9_-]{1,20}", action = @"\w+" });
            routes.MapRoute(
                name: "Comments",
                url: "game/{key}/{action}",
                defaults: new { controller = "Comments", action = @"Details" },
                constraints: new { key = @"^[A-Za-z0-9_-]{1,20}" });
            routes.MapRoute(
                name: "Games",
                url: "games/{action}",
                defaults: new { controller = "Games", action = @"\w+" });
            routes.MapRoute(
               name: "Publishers",
               url: "publisher/{action}",
               defaults: new { controller = "Publishers", action = @"\w+" });
            routes.MapRoute(
                name: "Basket",
                url: "basket/{gameId}",
                defaults: new { controller = "Orders", action="AddToBusket", gameId = @"^[A-Za-z0-9_-]{1,20}" , quantity=@"^(/d){1,3}", customerId= "^[A-Za-z0-9_-]{0,20}" }
                );
            routes.MapRoute(
                name: "Default",
                url: string.Empty,
                defaults: new { controller = "Games", action = "Index", id = string.Empty });

        }
    }
}
