using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Games", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Index",
            //    url: "games",
            //    defaults: new { controller = "Games", action = "Index", id = UrlParameter.Optional },
            //    constraints: new { httpMethod = new HttpMethodConstraint("GET") });

            //routes.MapRoute(
            //    name: "Index1",
            //    url: "game/{gamekey}",
            //    defaults: new { controller = "Game", action = "Index", id = UrlParameter.Optional },
            //    constraints: new { httpMethod = new HttpMethodConstraint("GET") });

            routes.MapRoute(
                name: "Comments",
                url: "game/{gamekey}/comments",
                defaults: new { controller = "Game", action = "Comments", id = UrlParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") });

            routes.MapRoute(
                name: "DownloadGame",
                url: "game/{gamekey}/download",
                defaults: new { controller = "Games", action = "Download", id = UrlParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") });

            //routes.MapRoute(
            //    name: "GamesCRUD",
            //    url: "games/{action}",
            //    defaults: new { controller = "Games", id = UrlParameter.Optional },
            //    constraints: new { action = "^(new|update|remove)", httpMethod = new HttpMethodConstraint("POST") });

            routes.MapRoute(
                name: "NewComment",
                url: "game/{gamekey}/newcomment",
                defaults: new { controller = "Game", action = "NewComment", id = UrlParameter.Optional },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "",
            //    defaults: new { controller = "Games", action = "Index", id = UrlParameter.Optional }
            //);





        }
    }
}
