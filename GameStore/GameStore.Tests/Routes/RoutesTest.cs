using System.Web.Routing;
using NUnit.Framework;

namespace GameStore.Web.Tests.Routes
{
    [TestFixture]
    public class RoutesTests
    {
        [Test]
        public void RouteWithControllerNoActionNoId()
        {
            // Arrange
            var context = new StubHttpContextForRouting(requestUrl: "~/games");
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act
            RouteData routeData = routes.GetRouteData(context);

            // Assert
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Games", routeData.Values["controller"]);
            Assert.AreEqual("Index", routeData.Values["action"]);
        }
        [Test]
        public void RouteWithControllerWithActionNoId()
        {
            // Arrange
            var context = new StubHttpContextForRouting(requestUrl: "~/games/action2");
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act
            RouteData routeData = routes.GetRouteData(context);

            // Assert
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Games", routeData.Values["controller"]);
            Assert.AreEqual("action2", routeData.Values["action"]);

        }
        [Test]
        public void RouteWithControllerWithActionWithId()
        {
            // Arrange
            var context = new StubHttpContextForRouting(requestUrl: "~/game/key/action1");
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act
            RouteData routeData = routes.GetRouteData(context);

            // Assert
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Game", routeData.Values["controller"]);
            Assert.AreEqual("action1", routeData.Values["action"]);
            Assert.AreEqual("key", routeData.Values["id"]);
        }
        [Test]
        public void RouteForDownload()
        {
            // Arrange
            var context = new StubHttpContextForRouting(requestUrl: "~/game/Gta6_ThirdEdition/download");
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act
            RouteData routeData = routes.GetRouteData(context);

            // Assert
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Games", routeData.Values["controller"]);
            Assert.AreEqual("Download", routeData.Values["action"]);
            Assert.AreEqual("Gta6_ThirdEdition", routeData.Values["id"]);
        }
        [Test]
        public void RouteWithTooManySegments()
        {
            // Arrange
            var context = new StubHttpContextForRouting(requestUrl: "~/a/b/c/d");
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act
            RouteData routeData = routes.GetRouteData(context);

            // Assert
            Assert.IsNull(routeData);
        }
        [Test]
        public void RouteForDetails()
        {
            // Arrange
            var context = new StubHttpContextForRouting(requestUrl: "~/game/Gta6_ThirdEdition");
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act
            RouteData routeData = routes.GetRouteData(context);

            // Assert
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Game", routeData.Values["controller"]);
            Assert.AreEqual("Details", routeData.Values["action"]);
            Assert.AreEqual("Gta6_ThirdEdition", routeData.Values["id"]);
        }
        [Test]
        public void DefaultRoute()
        {
            // Arrange
            var context = new StubHttpContextForRouting(requestUrl: "~/");
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act
            RouteData routeData = routes.GetRouteData(context);

            // Assert
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Games", routeData.Values["controller"]);
            Assert.AreEqual("Index", routeData.Values["action"]);

        }
    }
}
