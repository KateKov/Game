﻿using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "GetGames",
                url: "games",
                defaults: new { controller = "Games", action = "Index" }
            );
            routes.MapRoute(
             name: "DownloadGame",
             url: "game/{id}/download",
             defaults: new { controller = "Games", action = "Download" },
             constraints: new { id = @"^[A-Za-z0-9_-]{1,20}" }
            );

            routes.MapRoute(
                name: "GameMain",
                url: "game/{id}/{action}",
                defaults: new { controller = "Game" },
                constraints: new { id = @"^[A-Za-z0-9_-]{1,20}", action = @"\w+" }
            );

            routes.MapRoute(
                name: "Game",
                url: "game/{id}",
                defaults: new { controller = "Game", action = "Details" },
                constraints: new { id = @"^[A-Za-z0-9_-]{1,20}" }
            );
            routes.MapRoute(
            name: "Route",
            url: "games/{action}",
            defaults: new { controller = "Games", action = @"\w+" }
        );

            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { controller = "Games", action = "Index", id = "" }
                );

        }
    }
}