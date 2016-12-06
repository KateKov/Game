using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Model;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GameStore.DAL.EF;
using GameStore.DAL.Interfaces;
using GameStore.DAL.MongoEntities;
using GameStore.Web.Infrastracture;
using GameStore.Web.App_Start;

namespace GameStore.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //Database.SetInitializer(new GameStoreDbInitializer());
            FilterConfig.RegisterFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Configure();
        }
    }
}
