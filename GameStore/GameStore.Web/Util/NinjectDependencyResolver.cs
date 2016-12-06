using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
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
            _kernel.Bind<IGameStoreService>().To<GameStoreService>();
            _kernel.Bind<ICommentService>().To<CommentsService>();
            _kernel.Bind(typeof(IService<>)).To(typeof(Service<,>));
            _kernel.Bind(typeof(INamedService<,>)).To(typeof(ModelWithNameService<,,,>));
            _kernel.Bind(typeof(IWithKeyService<>)).To(typeof(ModelWithKeyService<,>));
            _kernel.Bind<IGameService>().To<GamesService>();
            _kernel.Bind<IOrderService>().To<OrdersService>();
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