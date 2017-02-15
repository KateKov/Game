using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GameStore.Web.Util;

namespace GameStore.Web
{
    public class RouteConfig
    {
        private const string DefaultLanguage = "en";

        public static void RegisterRoutes(RouteCollection routes)
        {
            //Games
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "GamesDefaultLang",
                url: "{lang}/games",
                defaults: new {controller = "Games", action = "Index"},
                constraints: new {lang = @"en|ru"});
            routes.MapRoute(
                name: "GamesLang",
                url: "{lang}/games/{action}",
                defaults: new {controller = "Games", action = @"\w+"},
                constraints: new {lang = @"en|ru"});
            routes.MapRoute(
                name: "GamesUpdateLang",
                url: "{lang}/game/{key}/update",
                defaults: new {controller = "Games", action = "Update"},
                constraints: new {key = @"^[A-Za-z0-9_-]{1,100}", lang = @"en|ru"});

            //Comments
            routes.MapRoute(
                name: "CommentsDeleteLang",
                url: "{lang}/game/{key}/comment/{commentId}",
                defaults: new {controller = "Comments", action = "Delete", lang = DefaultLanguage},
                constraints: new {key = @"^[A-Za-z0-9_-]{1,1000}", action = "Delete", lang = @"en|ru"});

            routes.MapRoute(
                name: "CommentsBanLang",
                url: "{lang}/game/{key}/{action}",
                defaults: new {controller = "Comments", lang = DefaultLanguage},
                constraints: new {key = @"^[A-Za-z0-9_-]{1,1000}", action = @"\w+", lang = @"en|ru"});
            routes.MapRoute(
                name: "CommentsDefaultLang",
                url: "{lang}/game/{key}/ban/{name}",
                defaults: new {controller = "Comments", action = "Ban", lang = DefaultLanguage},
                constraints: new {key = @"^[A-Za-z0-9_-]{1,300}", lang = @"en|ru"});


            //Accounts
            routes.MapRoute(
                name: "AuthRouteLang",
                url: "{lang}/Account/{action}",
                defaults:
                new {controller = "Account", action = "Login", key = UrlParameter.Optional, lang = DefaultLanguage},
                constraints: new {lang = @"en|ru"});

            //Users
            routes.MapRoute(
                name: "UsersDefaultLang",
                url: "{lang}/users",
                defaults: new {controller = "Users", action = "Index"},
                constraints: new {lang = @"en|ru"});
            routes.MapRoute(
                name: "UserNew",
                url: "{lang}/users/new",
                defaults: new {controller = "Users", action = "New"},
                constraints: new {lang = @"en|ru"});
            routes.MapRoute(
                name: "UserLang",
                url: "{lang}/user/{name}/{action}",
                defaults: new {controller = "Users", action = @"\w+"},
                constraints: new {lang = @"en|ru"});

            //Roles
            routes.MapRoute(
                name: "RolesDefaultLang",
                url: "{lang}/roles",
                defaults: new {controller = "Roles", action = "Index"},
                constraints: new {lang = @"en|ru"});
            routes.MapRoute(
                name: "RoleNew",
                url: "{lang}/roles/new",
                defaults: new {controller = "Roles", action = "New"},
                constraints: new {lang = @"en|ru"});
            routes.MapRoute(
                name: "RoleLang",
                url: "{lang}/role/{name}/{action}",
                defaults: new {controller = "Roles", action = @"\w+"},
                constraints: new {lang = @"en|ru"});


            //Publishers
            routes.MapRoute(
                name: "PublishersDefaultLang",
                url: "{lang}/publishers",
                defaults: new {controller = "Publishers", action = "Index"},
                constraints: new {lang = @"en|ru"});
            routes.MapRoute(
                name: "PublisherNew",
                url: "{lang}/publishers/new",
                defaults: new {controller = "Publishers", action = "New"},
                constraints: new {lang = @"en|ru"});
            routes.MapRoute(
                name: "PublisherLang",
                url: "{lang}/publisher/{name}/{action}",
                defaults: new {controller = "Publishers", action = @"\w+"},
                constraints: new {lang = @"en|ru"});

            //Genres
            routes.MapRoute(
                name: "GenresDefaultLang",
                url: "{lang}/genres",
                defaults: new {controller = "Genres", action = "Index"},
                constraints: new {lang = @"en|ru"});
            routes.MapRoute(
                name: "GenresNew",
                url: "{lang}/genres/New",
                defaults: new {controller = "Genres", action = "New"},
                constraints: new {lang = @"en|ru"});
            routes.MapRoute(
                name: "GenresLang",
                url: "{lang}/genre/{name}/{action}",
                defaults: new {controller = "Genres", action = @"\w+"},
                constraints: new {name = @"^[A-Za-zа-яА-Я\s0-9_-]{1,50}", lang = @"en|ru"});

            //PlatformTypes
            routes.MapRoute(
                name: "TypesDefaultLang",
                url: "{lang}/types",
                defaults: new {controller = "PlatformTypes", action = "Index"},
                constraints: new {lang = @"en|ru"});
            routes.MapRoute(
                name: "TypesNew",
                url: "{lang}/types/New",
                defaults: new {controller = "PlatformTypes", action = "New"},
                constraints: new {lang = @"en|ru"});
            routes.MapRoute(
                name: "TypesLang",
                url: "{lang}/type/{name}/{action}",
                defaults: new {controller = "PlatformTypes", action = @"\w+"},
                constraints: new {name = @"^[A-Za-zа-яА-Я\s0-9_-]{1,50}", lang = @"en|ru"});


            //Orders
            routes.MapRoute(
                name: "OrderNewLang",
                url: "{lang}/orders",
                defaults: new {controller = "Orders", action = "OrderNew", lang = DefaultLanguage},
                constraints: new {lang = @"en|ru", action = "OrderNew"});
            routes.MapRoute(
                name: "OrderLang",
                url: "{lang}/order/{username}",
                defaults: new {controller = "Orders", action = "Order", lang = DefaultLanguage},
                constraints: new {lang = @"en|ru", action = "Order"});

            routes.MapRoute(
                name: "OrderPaymentUserLang",
                url: "{lang}/order/{username}/{orderId}/{paymentName}/{action}",
                defaults: new {controller = "Orders", lang = DefaultLanguage},
                constraints: new {lang = @"en|ru", action = "Pay", paymentName = @"Bank|Visa|IBox"});
            routes.MapRoute(
                name: "OrderUserLang",
                url: "{lang}/order/{username}/{action}",
                defaults: new {controller = "Orders", lang = DefaultLanguage},
                constraints: new {lang = @"en|ru", action = @"\w+"});

            routes.MapRoute(
                name: "OrderDefaultLang",
                url: "{lang}/orders/{action}",
                defaults: new {controller = "Orders", lang = DefaultLanguage},
                constraints: new {lang = @"en|ru", action = @"\w+"});
            routes.MapRoute(
                name: "OrderDetailsLang",
                url: "{lang}/orders/{username}/detail/{orderDetailId}",
                defaults: new {controller = "Orders", lang = DefaultLanguage},
                constraints: new {lang = @"en|ru", action = @"\w+"});

            //Basket
            routes.MapRoute(
                name: "BasketLang",
                url: "{lang}/basket/{username}",
                defaults: new {controller = "Orders", action = "Basket", lang = DefaultLanguage},
                constraints: new {lang = @"en|ru", action = "Basket"});

            //Default
            routes.MapRoute(
                name: "Default",
                url: string.Empty,
                defaults: new {controller = "Games", action = "Index", id = string.Empty, lang = DefaultLanguage});

            routes.Add(
                "game",
                new Route("Game/{key}", new CustomRouteHandler()));
        }

        class CustomRouteHandler : IRouteHandler
        {
            public IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return new HttpHandler();
            }
        }
    }
}