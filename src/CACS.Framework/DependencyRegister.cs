using CACS.Framework.Data;
using CACS.Framework.Identity;
using CACS.Framework.Mvc;
using CACSLibrary.Data;
using CACSLibrary.Infrastructure;
using CACSLibrary.Plugin;
using CACSLibrary.Profile;
using CACSLibrary.Web;
using CACSLibrary.Web.EmbeddedViews;
using CACSLibrary.Web.Routes;
using Microsoft.AspNet.Identity.Owin;
using System.Web;

namespace CACS.Framework
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            //http
            containerManager.RegisterDelegate<HttpContextBase>(c => HttpContext.Current != null ? (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) : (new NullHttpContext("~/") as HttpContextBase), ComponentLifeStyle.LifetimeScope);
            containerManager.RegisterDelegate<HttpRequestBase>(c => c.Resolve<HttpContextBase>().Request, ComponentLifeStyle.LifetimeScope);
            containerManager.RegisterDelegate<HttpResponseBase>(c => c.Resolve<HttpContextBase>().Response, ComponentLifeStyle.LifetimeScope);
            containerManager.RegisterDelegate<HttpServerUtilityBase>(c => c.Resolve<HttpContextBase>().Server, ComponentLifeStyle.LifetimeScope);
            containerManager.RegisterDelegate<HttpSessionStateBase>(c => c.Resolve<HttpContextBase>().Session, ComponentLifeStyle.LifetimeScope);

            //mvc
            containerManager.RegisterComponentInstance<IRoutePublisher>(new RoutePublisher(typeFinder));
            containerManager.RegisterComponentInstance<IEmbeddedViewResolver>(new EmbeddedViewResolver(typeFinder));
            containerManager.RegisterComponent<IWebHelper, WebHelper>(typeof(WebHelper).FullName, ComponentLifeStyle.LifetimeScope);

            //plugin
            if (!Singleton<IPluginManager>.IsInstanceNull)
                EngineContext.Current.ContainerManager.RegisterComponentInstance<IPluginManager>(Singleton<IPluginManager>.Instance);
            containerManager.RegisterComponent<IPluginFinder, PluginFinder>("", ComponentLifeStyle.LifetimeScope);

            //data
            containerManager.RegisterDelegate<IDataSettingsManager>(c => new DataSettingsManager(c.Resolve<IProfileManager>()), ComponentLifeStyle.Singleton);
            containerManager.RegisterDelegate<BaseDataProviderManager>(c => new WebEfDataProviderManager(c.Resolve<IDataSettingsManager>().LoadSettings()), ComponentLifeStyle.Singleton);
            containerManager.RegisterDelegate<IDataProvider>(c => c.Resolve<BaseDataProviderManager>().LoadDataProvider(), ComponentLifeStyle.Singleton);
            containerManager.RegisterDelegate<IEfDataProvider>(c => c.Resolve<BaseDataProviderManager>().LoadDataProvider() as IEfDataProvider);
            containerManager.Resolve<IEfDataProvider>().InitConnectionFactory();
            containerManager.RegisterComponent<IDbContext, CACSWebObjectContext>(typeof(CACSWebObjectContext).FullName, ComponentLifeStyle.LifetimeScope);
            containerManager.RegisterComponent(typeof(IRepository<>), typeof(EfRepository<>), typeof(EfRepository<>).FullName, ComponentLifeStyle.LifetimeScope);

            //account
            containerManager.RegisterDelegate<ApplicationUserManager>(c => c.Resolve<HttpContextBase>().GetOwinContext().Get<ApplicationUserManager>(), ComponentLifeStyle.LifetimeScope);
            containerManager.RegisterDelegate<ApplicationRoleManager>(c => c.Resolve<HttpContextBase>().GetOwinContext().Get<ApplicationRoleManager>(), ComponentLifeStyle.LifetimeScope);
            containerManager.RegisterDelegate<ApplicationSignInManager>(c => c.Resolve<HttpContextBase>().GetOwinContext().Get<ApplicationSignInManager>(), ComponentLifeStyle.LifetimeScope);
        }

        public EngineLevels Level
        {
            get { return EngineLevels.Priority; }
        }
    }
}
