using CACSLibrary.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CACS.Framework.Mvc
{
    [ModelBinder(typeof(CACSModelBinder))]
    public abstract class BaseModel
    {
        public virtual object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return EngineContext.Current.Resolve<IModelProvider>().BuildModel(controllerContext, bindingContext);
        }
    }
}
