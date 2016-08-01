using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CACS.Framework.Mvc.ActionResults
{
    public class ExceptionResult : CACSResult
    {
        public Exception Exception
        {
            get;
            protected set;
        }

        public ExceptionResult(Exception ex)
        {
            Exception = ex;
        }

        protected override ActionResult GetAjaxResult(ControllerContext context)
        {
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                ContentType = "text/html",
                ContentEncoding = System.Text.Encoding.Unicode,
#if DEBUG
                Data = new JsonData(false, Exception.ToString())
#else
                Data = new JsonData(false, Exception.Message)
#endif
            };
        }

        protected override ActionResult GetRequestResult(ControllerContext context)
        {
            var controllerName = (string)context.RouteData.Values["controller"];
            var actionName = (string)context.RouteData.Values["action"];
            var model = new HandleErrorInfo(Exception, controllerName, actionName);
            return new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model),
                TempData = context.Controller.TempData
            };
        }
    }
}
