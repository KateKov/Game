using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.DAL.EF;
using GameStore.DAL.Entities;

namespace GameStore.Filters
{
    public class CustomActionFilter: ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)


        {
            // TODO: Add your acction filter's tasks here

            // Log Action Filter Call
            GameStoreContext storeDB = new GameStoreContext();

            ActionLog log = new ActionLog()
            {
                Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                Action = filterContext.ActionDescriptor.ActionName + " (Logged By: Custom Action Filter)",
                IP = filterContext.HttpContext.Request.UserHostAddress,
                DateTime = filterContext.HttpContext.Timestamp
        };

        storeDB.ActionLogs.Add(log);
        storeDB.SaveChanges();

        this.OnActionExecuting(filterContext);
    }

}
}