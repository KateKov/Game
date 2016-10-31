using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Web.WebPages;

namespace GameStore.Tests.Routes
{
    [TestFixture]
    public class RoutesTest
    {
        private readonly RouteCollection _routes;

        public RoutesTest()
        {
            _routes = RouteTable.Routes;
            RouteConfig.RegisterRoutes(_routes);
        }

        [Test]
        [TestCase("~/", "get", "Games", "Index")]
        [TestCase("~/games", "get", "Games", "Index")]
        [TestCase("~/game/index", "get", "Game", "Index")]
        [TestCase("~/game/{gamekey}/comments", "get", "Game", "Comments")]
        [TestCase("~/game/{gamekey}/download", "get", "Game", "Download")]
        [TestCase("~/game/{gamekey}/newcomment", "post", "Game", "NewComment")]
        [TestCase("~/games/new", "post", "Games", "New")]
        [TestCase("~/games/update", "post", "Games", "Update")]
        [TestCase("~/games/remove", "post", "Games", "Remove")]
        public void DefoultRoute(string url, string httpMethod, string expectedController, string expectedAction)
        {
            // Arrange
            var context = new StubHttpContextForRouting(requestUrl: url, httpMethod: httpMethod);

            // Act
            RouteData routeData = _routes.GetRouteData(context);

            // Assert
            Assert.IsNotNull(routeData);
            Assert.AreEqual(expectedController.ToUpper(), ((string)routeData.Values["controller"]).ToUpper());
            Assert.AreEqual(expectedAction.ToUpper(), ((string)routeData.Values["action"]).ToUpper());
        }
    }
}
