using CACS.Framework.Mvc.ActionResults;
using CACSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CACS.Framework.Mvc.Filters
{
    public class ModelValidateAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                var errors = filterContext.Controller.ViewData.ModelState.ToArray();
                string message = string.Join(",", errors);
                filterContext.Result = new ExceptionResult(new CACSException(message));
            }
        }
    }
}
