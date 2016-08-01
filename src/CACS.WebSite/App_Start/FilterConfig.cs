using CACS.Framework.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CACS.WebSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExceptionAttribute()
            {
                Order = int.MaxValue
            });
            filters.Add(new ModelValidateAttribute()
            {
                Order = -1
            });
            //filters.Add(new CrossDomainAttribute());
            //filters.Add(new HandleModelVerifyAttribute());
            //filters.Add(new ValidateReHttpPostTokenAttribute());
            //filters.Add(new AdminTicketAttribute());
        }
    }
}