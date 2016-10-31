using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using GameStore.Infrastracture.Diagnostics;
using NLog;

namespace GameStore.Infrastracture.DI
{
    public class WebAppModule: NinjectModule
    {
        public override void Load()
        {
            Bind<ILog>().ToMethod(x =>
            {
                var scope = x.Request.ParentRequest.Service.FullName;
                var log = (ILog)LogManager.GetLogger(scope, typeof(Log));
                return log;
            });
            /* other code omitted for brevity */
        }
    }
}