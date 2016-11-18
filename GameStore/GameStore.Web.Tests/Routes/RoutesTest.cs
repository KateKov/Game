using System.Web.Routing;
using NUnit.Framework;

namespace GameStore.Web.Tests.Routes
{
    [TestFixture]
    public class RoutesTests
    {
        [Test]
        public void RouteWithController_NoAction_NoId()
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
        public void RouteWithController_WithAction_NoId()
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
        public void RouteWithController_WithAction_WithId()
        {
            // Arrange
            var context = new StubHttpContextForRouting(requestUrl: "~/game/key/action1");
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act
            RouteData routeData = routes.GetRouteData(context);

            // Assert
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Comments", routeData.Values["controller"]);
            Assert.AreEqual("action1", routeData.Values["action"]);
            Assert.AreEqual("key", routeData.Values["key"]);
        }

        [Test]
        public void RouteForDownload()
        {
            // Arrange
            var context = new StubHttpContextForRouting(requestUrl: "~/game/Gta6_ThirdEdition/Download");
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act
            RouteData routeData = routes.GetRouteData(context);

            // Assert
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Comments", routeData.Values["controller"]);
            Assert.AreEqual("Download", routeData.Values["action"]);
            Assert.AreEqual("Gta6_ThirdEdition", routeData.Values["key"]);
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
            Assert.AreEqual("Comments", routeData.Values["controller"]);
            Assert.AreEqual("Details", routeData.Values["action"]);
            Assert.AreEqual("Gta6_ThirdEdition", routeData.Values["key"]);
        }

        [Test]
        public void DefaultRoute()
        {
            // Arrange
            var context = new StubHttpContextForRouting("~/");
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
