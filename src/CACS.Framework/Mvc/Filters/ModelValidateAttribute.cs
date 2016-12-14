using CACS.Framework.Mvc.ActionResults;
using CACSLibrary;
using CACSLibrary.Data;
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
                List<string> errors = new List<string>();
                var states = filterContext.Controller.ViewData.ModelState.ToArray();
                states.ForEach(state =>
                {
                    state.Value.Errors.ForEach(e => errors.Add(e.ErrorMessage));
                });
                string message = string.Join(",", errors.ToArray());
                filterContext.Result = new ExceptionResult(new CACSException(message));
            }
        }
    }
}
