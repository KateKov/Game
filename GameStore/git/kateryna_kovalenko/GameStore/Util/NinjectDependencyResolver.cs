using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services;
using GameStore.DAL.Infrastracture;
using NLog;

namespace GameStore.Util
{
    public class NinjectDependencyResolver :IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
           
            kernel.Bind<IService>().To<GameService>();
 
            kernel.Bind<ILogger>().ToMethod(p =>
            {
                if (p.Request.Target != null && p.Request.Target.Member.DeclaringType != null)
                   return LogManager.GetLogger(p.Request.Target.Member.DeclaringType.ToString());
               return LogManager.GetLogger("Unknown action");
           });
       }

    }
}