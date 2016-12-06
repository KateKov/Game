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
                constraints: new { key = @"^[A-Za-z0-9_-]{1,30}", action = @"\w+" });
            routes.MapRoute(
                name: "Comments",
                url: "game/{key}/{action}",
                defaults: new { controller = "Comments", action = @"Details" },
                constraints: new { key = @"^[A-Za-z0-9_-]{1,50}" });
            routes.MapRoute(
                name: "Games",
                url: "games/{action}",
                defaults: new { controller = "Games", action = @"\w+" });
            routes.MapRoute(
                name: "Publishers",
                url: "publisher/{action}",
                defaults: new {controller = "Publishers", action = @"\w+"});
            routes.MapRoute(
            name: "OrdersHistory",
            url: "orders/history",
            defaults: new { controller = "Orders", action = "History" }
            );
            routes.MapRoute(
             name: "Order",
             url: "order",
             defaults: new { controller = "Orders", action = "Order" }
             );
            routes.MapRoute(
            name: "Basket",
            url: "basket",
             defaults: new { controller = "Orders", action = "Basket", customerId ="" }
            );
            routes.MapRoute(
              name: "Orders",
              url: "orders/{action}",
              defaults: new { controller = "Orders", action = @"\w+" });
            routes.MapRoute(
         name: "Shipper",
         url: "shipper",
          defaults: new { controller = "Shipper", action = "Index", customerId = "" }
         );

            routes.MapRoute(
                name: "Default",
                url: string.Empty,
                defaults: new { controller = "Games", action = "Index", id = string.Empty });

        }
    }
}
