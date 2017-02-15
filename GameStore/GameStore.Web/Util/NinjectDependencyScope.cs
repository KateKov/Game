using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject.Syntax;
using Ninject.Activation;
using System.Linq;
using Ninject.Parameters;

namespace GameStore.Web.Util
{
    public class NinjectDependencyScope : IDependencyScope
    {
        private readonly IResolutionRoot _resolutionRoot;

        public NinjectDependencyScope(IResolutionRoot kernel)
        {
            _resolutionRoot = kernel;
        }

        public object GetService(Type serviceType)
        {
            IRequest request = _resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return _resolutionRoot.Resolve(request).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IRequest request = _resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return _resolutionRoot.Resolve(request).ToList();
        }

        public void Dispose()
        {
        }
    }
}