using CACSLibrary.Infrastructure;
using CACSLibrary.Web.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CACS.WebSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.svc/{*pathInfo}");

            //IRoutePublisher publisher = EngineContext.Current.Resolve<IRoutePublisher>();
            //publisher.RegisterRoutes(RouteTable.Routes);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CACS.WebSite.Controllers" }
            );

            routes.MapRoute(
                name: "Error",
                url: "Home/Error"
            );
        }
    }
}
