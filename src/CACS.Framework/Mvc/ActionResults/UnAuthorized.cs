using CACSLibrary;
using System.Web.Mvc;

namespace CACS.Framework.Mvc.ActionResults
{
    public class UnAuthorized : CACSResult
    {
        string _controller;
        string _action;

        public UnAuthorized(string controller, string action)
        {
            _controller = controller;
            _action = action;
        }

        protected override ActionResult GetAjaxResult(ControllerContext context)
        {
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new JsonData(false, string.Format("没有操作权限：{0} - {1}", _controller, _action))
            };
        }

        protected override ActionResult GetRequestResult(ControllerContext context)
        {
            var controllerName = (string)context.RouteData.Values["controller"];
            var actionName = (string)context.RouteData.Values["action"];
            var model = new HandleErrorInfo(new CACSException("没有权限"), controllerName, actionName);
            return new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model),
                TempData = context.Controller.TempData
            };
        }
    }
}
