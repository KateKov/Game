using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services;
using GameStore.DAL.Infrastracture;
using GameStore.DAL.Interfaces;
using Ninject;
using NLog;

namespace GameStore.Web.Util
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IService>().To<GameStoreService>();
            _kernel.Bind<ILogger>().ToMethod(p =>
            {
                if (p.Request.Target != null && p.Request.Target.Member.DeclaringType != null)
                    return LogManager.GetLogger(p.Request.Target.Member.DeclaringType.ToString());
                return LogManager.GetLogger("Unknown action");
            });
            _kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}