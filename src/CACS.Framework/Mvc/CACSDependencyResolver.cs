using CACSLibrary.Infrastructure;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CACS.Framework.Mvc
{
    public class CACSDependencyResolver : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            return EngineContext.Current.ContainerManager.IsRegistered(serviceType) ? EngineContext.Current.ContainerManager.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(serviceType);
            return (IEnumerable<object>)EngineContext.Current.Resolve(type);
        }
    }
}
