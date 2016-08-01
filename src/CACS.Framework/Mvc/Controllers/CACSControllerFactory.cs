using CACSLibrary.Infrastructure;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace CACS.Framework.Mvc.Controllers
{
    public class CACSControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            var isRegisted = EngineContext.Current.ContainerManager.IsRegistered(controllerType);
            if (!isRegisted)
                EngineContext.Current.ContainerManager.RegisterComponent(
                    controllerType,
                    controllerType,
                    controllerType.FullName,
                    ComponentLifeStyle.LifetimeScope);
            return EngineContext.Current.ContainerManager.Resolve(controllerType) as IController;
        }
    }
}
