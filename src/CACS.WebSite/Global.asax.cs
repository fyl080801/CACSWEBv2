using CACS.Framework.Mvc;
using CACS.Framework.Mvc.Controllers;
using CACSLibrary.Configuration;
using CACSLibrary.Infrastructure;
using CACSLibrary.Web;
using CACSLibrary.Web.EmbeddedViews;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CACS.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
