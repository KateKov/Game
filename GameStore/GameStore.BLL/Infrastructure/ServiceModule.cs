using GameStore.DAL.Entities;
using GameStore.DAL.Infrastracture;
using GameStore.DAL.Interfaces;
using Ninject.Modules;

namespace GameStore.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule(string connection)
        {
            connectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(connectionString);
            Bind<IRepository<Game>>().To<Repository<Game>>().WithConstructorArgument(connectionString);
            Bind<IRepository<Genre>>().To<Repository<Genre>>().WithConstructorArgument(connectionString);
            Bind<IRepository<Comment>>().To<Repository<Comment>>().WithConstructorArgument(connectionString);
            Bind<IRepository<PlatformType>>().To<Repository<PlatformType>>().WithConstructorArgument(connectionString);
        }
    }
}
