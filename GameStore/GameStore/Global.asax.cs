using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using GameStore.DAL.EF;

namespace GameStore.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Database.SetInitializer(new GameStoreDbInitializer());      
        }
    }
}
