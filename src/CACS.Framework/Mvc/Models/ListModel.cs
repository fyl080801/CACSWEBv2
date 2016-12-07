using CACSLibrary.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CACS.Framework.Mvc.Models
{
    public class ListModel : BaseModel
    {
        public string Search { get; set; } = "";

        public int Limit { get; set; } = 0;

        public int Page { get; set; } = 0;

        public Dictionary<string, string> Sort { get; set; } = new Dictionary<string, string>();

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return EngineContext.Current.Resolve<IListModelProvider>().BuildModel(controllerContext, bindingContext);
        }
    }
}
