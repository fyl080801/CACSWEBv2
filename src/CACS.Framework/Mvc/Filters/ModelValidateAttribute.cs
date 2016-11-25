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
        bool _isValid;

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!_isValid)
            {
                filterContext.Result = new ExceptionResult(new CACSException(filterContext.Controller.ViewData.ModelState.ToString()));
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _isValid = filterContext.Controller.ViewData.ModelState.IsValid;
            //if (!filterContext.Controller.ViewData.ModelState.IsValid)
            //{
            //    throw new CACSException(filterContext.Controller.ViewData.ModelState.ToString());
            //}
        }
    }
}
