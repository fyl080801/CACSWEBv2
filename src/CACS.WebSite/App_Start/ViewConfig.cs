using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace CACS.WebSite
{
    public class ViewConfig
    {
        public static void Register(VirtualPathProvider provider)
        {
            HostingEnvironment.RegisterVirtualPathProvider(provider);
        }
    }
}