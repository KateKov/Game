using System.Web.Http;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Infrastructure.MailServer;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.MailServer;
using GameStore.BLL.Interfaces.Services;
using GameStore.BLL.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Infrastracture;
using GameStore.DAL.Interfaces;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Interfaces;
using GameStore.Web.PaymentService;
using NLog;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(GameStore.Web.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(GameStore.Web.NinjectWebCommon), "Stop")]

namespace GameStore.Web
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Util;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ICommentService>().To<CommentsService>();
            kernel.Bind<IEncryptionService>().To<EncryptionService>();
            kernel.Bind<IGameService>().To<GamesService>();
            kernel.Bind<INamedService<PublisherDTO, PublisherDTOTranslate>>().To<PublishersService>();
            kernel.Bind<INamedService<PlatformTypeDTO, PlatformTypeDTOTranslate>>().To<PlatformTypesService>();
            kernel.Bind<INamedService<GenreDTO, GenreDTOTranslate>>().To<GenresService>();
            kernel.Bind<INamedService<RoleDTO, RoleDTOTranslate>>().To<RolesService>();

            kernel.Bind(typeof(ITranslateService<,>)).To(typeof(TranslateService<,>));
            kernel.Bind(typeof(IService<>)).To(typeof(Service<,>));

            kernel.Bind<ITranslateService<Game, GameDTO>>().To<TranslateService<Game, GameDTO>>();
            kernel.Bind<ITranslateService<Genre, GenreDTO>>().To<TranslateService<Genre, GenreDTO>>();
            kernel.Bind<IPaymentService>().To<PaymentServiceClient>();
            kernel.Bind<IObservable>().To<MailServer>();
            kernel.Bind<IObserver>().To<MailNotification>();
            kernel.Bind<IObserver>().To<MobileAppNotification>();
            kernel.Bind<IObserver>().To<SmsNotification>();

            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IAuthenticationManager>().To<AuthenticationManager>().InRequestScope();

            kernel.Bind<IService<OrderDetailDTO>>().To<Service<OrderDetail, OrderDetailDTO>>();
            kernel.Bind<IOrderService>().To<OrdersService>();
            kernel.Bind<IDtoToDomainMapping>().To<DtoToDomainMapping>();
            kernel.Bind<ILogger>().ToMethod(p =>
            {
                if ((p.Request.Target != null) && (p.Request.Target.Member.DeclaringType != null))
                    return LogManager.GetLogger(p.Request.Target.Member.DeclaringType.ToString());
                return LogManager.GetLogger("Unknown action");
            });
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
        }        
    }
}
