using System.Web.Http.Dependencies;
using Ninject;


namespace GameStore.Web.Util
{
    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        private readonly IKernel _kernel;
        public NinjectDependencyResolver(IKernel kernelParam) : base(kernelParam)
        {
            _kernel = kernelParam;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(_kernel);
        }

    }
}