using System.Web.Mvc;

namespace CACS.Framework.Mvc.ActionResults
{
    public abstract class CACSResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            var httprequest = context.HttpContext.Request.Headers["X-Requested-With"];
            var jsonrequest = context.HttpContext.Request.Headers["Content-Type"];
            var acceptrequest = context.HttpContext.Request.Headers["Accept"];
            ActionResult result = (httprequest != null && httprequest.Contains("XMLHttpRequest"))
                || (jsonrequest != null && jsonrequest.Contains("application/json"))
                || (acceptrequest != null && acceptrequest.Contains("application/json"))
                ? GetAjaxResult(context)
                : GetRequestResult(context);
            result.ExecuteResult(context);
        }

        protected abstract ActionResult GetRequestResult(ControllerContext context);

        protected abstract ActionResult GetAjaxResult(ControllerContext context);
    }
}
