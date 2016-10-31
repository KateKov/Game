using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Infrastracture;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Infrastructure
{
    public class ServiceModule: NinjectModule
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
