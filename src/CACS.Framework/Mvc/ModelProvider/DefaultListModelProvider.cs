using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CACS.Framework.Mvc.ModelProvider
{
    public class DefaultListModelProvider : IListModelProvider
    {
        public object BuildModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return bindingContext.ModelMetadata.Model;
        }
    }
}
