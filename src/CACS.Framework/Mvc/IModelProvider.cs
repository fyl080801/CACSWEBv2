using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CACS.Framework.Mvc
{
    public interface IModelProvider
    {
        object BuildModel(ControllerContext controllerContext, ModelBindingContext bindingContext);
    }
}
