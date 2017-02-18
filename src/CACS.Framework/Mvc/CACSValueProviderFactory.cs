using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CACS.Framework.Mvc
{
    public class CACSValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new FormJsonValueProvider(controllerContext.HttpContext);
        }
    }
}
