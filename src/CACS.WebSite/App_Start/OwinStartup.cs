using CACS.Framework.Identity;
using CACS.Framework.Mvc;
using CACS.Framework.Mvc.Controllers;
using CACSLibrary.Configuration;
using CACSLibrary.Infrastructure;
using CACSLibrary.Web;
using Microsoft.Owin;
using Owin;
using System.Configuration;
using System.Web.Hosting;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(CACS.WebSite.OwinStartup))]
namespace CACS.WebSite
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            EngineContext.Reset(new CACSWebEngine());
            EngineContext.Current.Initialize(ConfigurationManager.GetSection("cacsConfig") as CACSConfig);
            
            ModelBinders.Binders.Add(typeof(BaseModel), new CACSModelBinder());
            DependencyResolver.SetResolver(new CACSDependencyResolver());
            ControllerBuilder.Current.SetControllerFactory(new CACSControllerFactory());

            ViewConfig.Register(EngineContext.Current.Resolve<VirtualPathProvider>());

            app.CreateIdentityExtensions(EngineContext.Current);
        }
    }
}