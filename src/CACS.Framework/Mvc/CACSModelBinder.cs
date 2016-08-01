using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CACS.Framework.Mvc
{
    public class CACSModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);
            if (model is BaseModel)
            {
                bindingContext.ModelMetadata.Model = model;
                model = ((BaseModel)model).BindModel(controllerContext, bindingContext);
            }
            return model;
        }
    }
}
